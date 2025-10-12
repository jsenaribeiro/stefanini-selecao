using System.Reflection;
using Api.Service.Controllers;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;
using Shouldly;
using Api.Service.Commands;
using Api.Domain;

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

   [Given(@"uma pessoa com dados mínimos de")]
   public void DadoUmaPessoaComDadosMinimosDe(Table table)
   {
      _context["restrito"] = true;

      foreach (var row in table.Rows)
      {
         _context["cadastrar"] = new CadastrarPessoaCommand
         (
            row["nome"],
            row["nascimento"]
         );
      }
   }

   [Given(@"uma pessoa com")]
   public void DadoUmaPessoaCom(Table table)
   {
      _context["restrito"] = false;

      foreach (var row in table.Rows)
      {
         _context["cadastrar"] = new CadastrarPessoaCommand
         (
            (row["sexo"] ?? " ")[0],
            row["nome"],
            row["email"],
            row["nascimento"],
            row["nacionalidade"],
            row["cpf"]
         );
      }
   }

   [Given(@"cujo ""(.*)"" é ""(.*)""")]
   public void DadoCujoE(string campo, string valor)
   {
      var pessoa = _context["cadastrar"] as CadastrarPessoaCommand;

      var pessoaProps = pessoa?.GetType()?.GetProperty(campo);

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
      _context["falhas"] = result.ValueOf<DomainError>();

      Console.WriteLine($"Status atual: {_context["status"]}");
   }

   [Then(@"retornará erro de ""(.*)"" ""(.*)""")]
   public void EntaoRetornaraErroDeInvalido(string campo, string invalidacao)
   {
      var falhas = _context["falhas"] as DomainError;

      if (falhas?.Invalids is not Invalid[] invalids)
         throw new Exception("Não contém invalidações com " + campo);

      var erro = invalidacao == "inválido"
         ? string.Format(Messages.INVALIDO, campo)
         : string.Format(Messages.OBRIGATORIO, campo);

      invalids.ShouldContain(x => x.error == erro);
      
      _context["status"].ShouldBe(400);
   }

   [Then(@"terá cadastrado")]
   public void EntaoTeraCadastrado(Table table)
   {
      var apenasCamposObrigatorios = (bool)_context["restrito"];

      foreach (var row in table.Rows)
      {
         var nome = row["nome"];
         var nascimento = row["nascimento"].ToDateOnly("dd/MM/yyyy");

         var pessoa = unitOfWork.Pessoas.Query
            .Where(x => x.Nascimento == nascimento)
            .Where(x => x.Nome == nome)
            .FirstOrDefaultAsync()
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


   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Sicrano");
   }
}