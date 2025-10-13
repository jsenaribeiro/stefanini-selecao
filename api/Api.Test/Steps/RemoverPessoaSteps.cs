using TechTalk.SpecFlow;

namespace Api.Test;

[Binding]
public class RemoverPessoaSteps : AbstractSteps
{
   private readonly ScenarioContext _context;

   public RemoverPessoaSteps(ScenarioContext sc)
   {
      _context = sc;
   }

   [Given(@"um cadastro com uma pessoa")]
   public void DadoUmCadastroComUmaPessoa(Table table)
   {
      _context.Pending();
   }

   [When(@"remover o cadastro do ""(.*)""")]
   public void QuandoRemoverOCadastroDo(string pessoa)
   {
      _context.Pending();
   }

   [Then(@"""(.*)"" não será mais listado")]
   public void EntaoNaoSeraMaisListado(string pessoa)
   {
      _context.Pending();
   }

   [Then(@"o retorno terá o status code (.*)")]
   public void EntaoORetornoTeraOStatusCode(int status)
   {
      _context.Pending();
   }

   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Sicrano");
   }
}