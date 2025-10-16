using Api.Domain;
using Api.Domain.Pessoas;
using Api.Service.Commands;
using Api.Service.Controllers;
using Api.Service.Results;
using Api.Test.Pessoas;
using Shouldly;
using TechTalk.SpecFlow;

namespace Api.Test;

[Binding]
public class RemoverPessoaSteps : AbstractPessoaSteps
{
   private readonly ScenarioContext _context;

   public RemoverPessoaSteps(ScenarioContext sc)
   {
      _context = sc;
      _context["pessoas"] = new List<Pessoa>();
   }

   [Given(@"um cadastro com uma pessoa")]
   public async Task DadoUmCadastroComUmaPessoa(Table table)
   {
      foreach (var pessoa in GetInstantiationOf(table))
      {
         await unitOfWork.Pessoas.CreateAsync(pessoa);
         _context.Get<List<Pessoa>>("pessoas").Add(pessoa);
      }
   }

   [When(@"remover o cadastro do ""(.*)""")]
   public async Task QuandoRemoverOCadastroDo(string pessoa)
   {
      var pessoas = _context.Get<List<Pessoa>>("pessoas");
      var pessoaId = pessoas.First(x => x.Nome == pessoa).Id;
      var controller = new PessoaController(provider);
      var command = new RemoverPessoaCommand(pessoaId);
      var result = await controller.Delete(pessoaId, command);

      _context["status"] = result.GetStatusCode();
      _context["falhas"] = result.ValueOf<ErrorResult>();
   }

   [When(@"remover de um id não existente")]
   public async Task QuandoRemoverDeUmIdNaoExistente()
   {
      var idInexistente = Guid.NewGuid();
      var controller = new PessoaController(provider);
      var command = new RemoverPessoaCommand(idInexistente);
      var result = await controller.Delete(idInexistente, command);

      _context["status"] = result.GetStatusCode();
      _context["falhas"] = result.ValueOf<ErrorResult>();
   }

   [Then(@"""(.*)"" não será mais listado")]
   public async Task EntaoNaoSeraMaisListado(string pessoa)
   {
      var encontrou = await unitOfWork.Pessoas
         .ExistsAsync(x => x.Nome == pessoa);

      encontrou.ShouldBe(false);
   }

   [Then(@"uma mensagem de pessoa não encontrada")]
   public void EntaoUmaMensagemDePessoaNaoEncontrada()
   {
      var falhas = _context.Get<ErrorResult>("falhas");
      var erro = string.Format(Messages.NAO_ENCONTRADO, "Pessoa");

      falhas.Message.ShouldBe(erro);
   }

   [Then(@"o retorno terá o status code (.*)")]
   public void EntaoORetornoTeraOStatusCode(int status)
   {
      _context.Get<int>("status").ShouldBe(status);
   }

   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Sicrano");
   }
}