using System.Reflection;
using Api.Service.Controllers;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;
using Shouldly;
using Api.Service.Commands;
using Api.Domain;

namespace Api.Test.Steps;

[Binding]
public class AlterarPessoaSteps
{
   private readonly ScenarioContext _context;

   public AlterarPessoaSteps(ScenarioContext sc)
   {
      _context = sc;
   }

   [Given(@"uma pessoa cadastrada com")]
   public void DadoUmaPessoaCadastradaCom(Table table)
   {
      _context.Pending();
   }

   [When(@"alterar o cadastro do ""(.*)"" com ""(.*)"" = ""(.*)""")]
   public void QuandoAlterarOCadastroDoCom(string pessoa, string campo, string valor)
   {
      _context.Pending();
   }

   [When(@"alterar o cadastro com um id não cadastrado")]
   public void QuandoAlterarOCadastroComUmIdNaoCadastrado()
   {
      _context.Pending();
   }

   [Then(@"retornará a mensagem de erro de ""(.*)""")]
   public void EntaoRetornaraAMensagemDeErroDe(string tipoErro)
   {
      _context.Pending();
   }

   [Then(@"retornará uma mensagem de erro de pessoa não encontrada")]
   public void EntaoRetornaraUmaMensagemDeErroDePessoaNaoEncontrada()
   {
      _context.Pending();
   }

   [Then(@"retornará o status code (.*)")]
   public void EntaoRetornaraOStatusCode(int statusCode)
   {
      _context.Pending();
   }
}