using System.Reflection;
using Api.Service.Controllers;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;
using Shouldly;
using Api.Service.Commands;
using Api.Domain;
using Api.Domain.Pessoas;
using Api.Service.Results;

namespace Api.Test.Pessoas;

[Binding]
public class AtualizarPessoaSteps : AbstractPessoaSteps
{
   private readonly ScenarioContext _context;

   public AtualizarPessoaSteps(ScenarioContext sc) => _context = sc;

   [Given(@"uma pessoa cadastrada com")]
   public async Task DadoUmaPessoaCadastradaCom(Table table)
   {
      foreach (var pessoa in GetInstantiationOf(table))
      {
         await unitOfWork.Pessoas.CreateAsync(pessoa);
         _context.Get<List<Pessoa>>("pessoas").Add(pessoa);
      }
   }

   [When(@"alterar o cadastro do (.*) com (.*) = (.*)")]
   public async Task QuandoAlterarOCadastroDoCom(string pessoa, string campo, object? valor)
   {
      if (valor?.ToString() == "null") valor = null;

      var id = _context.Get<List<Pessoa>>("pessoas").First(x => x.Nome == pessoa).Id;

      var command = new AlterarPessoaCommand(id);

      valor = GetValueOf<AlterarPessoaCommand>(campo, valor);

      command.GetType().GetProperty(campo)!.SetValue(command, valor);

      var controller = new PessoaController(provider);
      var result = await controller.Put(id, command);

      _context["status"] = result.GetStatusCode();
      _context["falhas"] = result.ValueOf<ErrorResult>();
      _context["result"] = result.ValueOf<PessoaResult>();
   }

   [When(@"alterar o cadastro com um id não cadastrado")]
   public async Task QuandoAlterarOCadastroComUmIdNaoCadastrado()
   {
      var idInexistente = Guid.NewGuid();
      var command = new AlterarPessoaCommand(idInexistente);

      var controller = new PessoaController(provider);
      var result = await controller.Put(idInexistente, command);

      _context["status"] = result.GetStatusCode();
      _context["falhas"] = result.ValueOf<ErrorResult>();
      _context["result"] = result.ValueOf<PessoaResult>();
   }

   [Then(@"retornará erro de (.*) com (.*) e (.*)")]
   public void EntaoRetornaraAMensagemDeErroDe(string tipo, string campo, string valor)
   {
      var invalidos = _context.Get<ErrorResult>("falhas").Invalids;

      var mensagem = tipo switch
      {
         "obrigatorio" => string.Format(Messages.OBRIGATORIO, campo),
         "invalidacao" => string.Format(Messages.INVALIDO, campo),
         _ => string.Format(Messages.DUPLICIDADE, campo, valor)
      };

      invalidos.ShouldContain(x => x.error == string.Format(mensagem, ""));
   }

   [Then(@"o cadastro do cenário terá o (.*) = (.*)")]
   public void EntaoOCadastroDoCenarioTeraO(string campo, string valor)
   {
      if (_context.Get<int>("status") >= 300)
         throw new Exception("Gerou erro, enquanto esperava sucesso");

      var result = _context.Get<PessoaResult>("result");
      var pessoa = unitOfWork.Pessoas.LoadAsync(result.Id).Result!;
      var member = pessoa.GetType().GetProperty(campo)!.GetValue(pessoa)?.ToString() ?? "";

      member.ToString().ShouldBe(valor);
   }

   [Then(@"retornará uma mensagem de erro de pessoa não encontrada")]
   public void EntaoRetornaraUmaMensagemDeErroDePessoaNaoEncontrada()
   {
      var falhas = _context.Get<ErrorResult>("falhas");
      var esperado = string.Format(Messages.NAO_ENCONTRADO, "Pessoa");

      falhas.Message.ShouldBe(esperado);
   }

   [Then(@"retornará o status code (.*)")]
   public void EntaoRetornaraOStatusCode(int statusCode)
   {
      _context.Get<int>("status").ShouldBe(statusCode);
   }

   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Beltrana");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Sicrano");
   }
}