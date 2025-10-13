using System.Reflection;
using Api.Service.Controllers;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;
using Shouldly;
using Api.Service.Commands;
using Api.Domain;
using Api.Domain.Pessoas;
using Api.Service.Results;

namespace Api.Test.Steps;

[Binding]
public class CadastrarPessoaSteps : AbstractSteps
{
   private readonly ScenarioContext _context;
   private readonly PessoaController _controller;

   public CadastrarPessoaSteps(ScenarioContext sc)
   {
      _context = sc;
      _controller = new PessoaController(provider);
   }

   private (DateOnly data, string nome) GetRowNomeAndData(TableRow row)
   {
      var nascimento = row["nascimento"];

      var data = string.IsNullOrEmpty(nascimento) == false
               ? nascimento.ToDateOnly("dd/MM/yyyy")
               : default;

      return (data, row["nome"]);
   }

   [Given(@"uma pessoa com dados mínimos de")]
   public void DadoUmaPessoaComDadosMinimosDe(Table table)
   {
      _context["restrito"] = true;

      foreach (var row in table.Rows)
      {
         var (data, nome) = GetRowNomeAndData(row);

         var cadastrar = new CadastrarPessoaCommand(nome, data);

         _context["cadastrar"] = cadastrar;
      }
   }

   [Given(@"uma pessoa com")]
   public void DadoUmaPessoaCom(Table table)
   {
      _context["restrito"] = false;

      foreach (var row in table.Rows)
      {
         var nome = row["nome"];
         var nascimento = row["nascimento"].ToDateOnly("dd/MM/yyyy");
         var sexo = row["sexo"].ToUpper() == "M" ? Sexo.M : Sexo.F;

         _context["cadastrar"] = new CadastrarPessoaCommand(nome, nascimento)
         {
            Sexo = sexo,
            CPF = row["cpf"],
            Email = row["email"],
            Nacionalidade = row["nacionalidade"]
         };
      }
   }

   [Given(@"cujo ""(.*)"" é ""(.*)""")]
   public void DadoCujoE(string campo, object valor)
   {
      var pessoa = _context["cadastrar"] as CadastrarPessoaCommand;

      var pessoaProps = pessoa?.GetType()?.GetProperty(campo);

      if (campo == "Nascimento")
      {
         if (valor is null) valor = default(DateOnly);
         else if (valor is string s && s == "") valor = default(DateOnly);
         else if (valor?.ToString() == "00/00/0000") valor = default(DateOnly);
         else valor = valor!.ToString()!.ToDateOnly("dd/MM/yyyy");         
      }

      if (pessoaProps is PropertyInfo props)
         props.SetValue(pessoa, valor);
   }

   [When(@"cadastrar a pessoa")]
   public async Task QuandoCadastrarAPessoa()
   {
      var command = _context["cadastrar"] as CadastrarPessoaCommand;
      if (command is null) throw new Exception("Command está nulo");

      var result = await _controller.Post(command);

      _context["status"] = result.GetStatusCode();
      _context["falhas"] = result.ValueOf<ErrorResult>();

      Console.WriteLine($"Status atual: {_context["status"]}");
   }

   [Then(@"retornará erro de ""(.*)"" ""(.*)""")]
   public void EntaoRetornaraErroDeInvalido(string campo, string invalidacao)
   {
      if ((int)_context["status"] < 300)
         throw new Exception("Esperado um cenário com erro");

      var falhas = _context["falhas"] as ErrorResult;

      if (falhas?.Invalids is not Invalid[] invalids)
         throw new Exception("Não contém invalidações com " + campo);

      var erro = invalidacao == "inválido"
         ? string.Format(Messages.INVALIDO, campo)
         : string.Format(Messages.OBRIGATORIO, campo);

      invalids.ShouldContain(x => x.error == erro);
   }

   [Then(@"terá cadastrado")]
   public void EntaoTeraCadastrado(Table table)
   {
      var apenasCamposObrigatorios = (bool)_context["restrito"];

      foreach (var row in table.Rows)
      {
         var nome = row["nome"];
         var nascimento = row["nascimento"].ToDateOnly("dd/MM/yyyy");

         var pessoa = unitOfWork.Pessoas
            .Where(x => x.Nascimento == nascimento)
            .FirstOrDefaultAsync(x => x.Nome == nome)
            .Result;

         pessoa.ShouldNotBeNull();
         pessoa.Nome.ShouldBe(nome);
         pessoa.Nascimento.ShouldBe(nascimento);

         if (apenasCamposObrigatorios) continue;

         pessoa.CPF.ShouldBe(row["cpf"]);
         pessoa.Email.ShouldBe(row["email"]);
         pessoa.Sexo.ToString().ShouldBe(row["sexo"]);
         pessoa.Nacionalidade.ShouldBe(row["nacionalidade"]);
      }
   }

   [Then(@"terá status (.*)")]
   public void EntaoTeraStatus(int status)
   {
      _context["status"].ShouldBe(status);
   }

   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Sicrano");
   }
}