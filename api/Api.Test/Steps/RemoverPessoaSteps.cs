using TechTalk.SpecFlow;

namespace Api.Test;

[Binding]
public class StepDefinitions
{
   private readonly ScenarioContext _context;

   public StepDefinitions(ScenarioContext sc)
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
}