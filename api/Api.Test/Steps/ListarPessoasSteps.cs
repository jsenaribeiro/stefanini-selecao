using Api.Domain.Pessoas;
using Api.Infrastructure.Values;
using Api.Service.Controllers;
using TechTalk.SpecFlow;
using Shouldly;
using Api.Service.Queries;

namespace Api.Test.Steps;

[Binding]
public class ListarPessoasSteps : AbstractSteps
{
   private readonly ScenarioContext _context;

   private readonly PessoaController _controller;

   private readonly Sort _sort = new("nascimento", Order.ASC);

   public ListarPessoasSteps(ScenarioContext sc)
   {
      _context = sc;
      _context["paginado"] = false;
      _controller = new PessoaController(provider!);
   }

   [Given(@"que os seguintes cadastros")]
   public async Task DadoQueOsSeguintesCadastros(Table table)
   {
      foreach (var row in table.Rows)
      {
         var nome = row["nome"];
         var nascimento = row["nascimento"] ?? "";
         var dataNascimento = nascimento.ToDateOnly("dd/MM/yyyy");
         var pessoa = new Pessoa(nome, dataNascimento);

         await unitOfWork!.Pessoas.SaveAsync(pessoa);
      }
   }

   [When(@"listar os cadastros")]
   public async Task QuandoListarOsCadastros()
   {
      var query = new ConsultarPessoasQuery(_sort);

      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PageList<Pessoa>>();
   }

   [When(@"filtra cadastros com ""(.*)""")]
   public async Task QuandoFiltraCadastrosCom(string nome)
   {
      var query = new ConsultarPessoasQuery(_sort, nome);
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PageList<Pessoa>>();
   }

   [When(@"listar com (.*) linhas por página")]
   public async Task QuandoListarComLinhasPorPagina(int linhasPorPagina)
   {
      _context["paginado"] = true;
      var query = new ConsultarPessoasQuery(new(linhasPorPagina, 1), _sort);
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PageList<Pessoa>>();
   }

   [When(@"listar ordenado de modo crescente")]
   public async Task QuandoListarOrdenadoDeModoCrescente()
   {
      var query = new ConsultarPessoasQuery(new Sort("nome", Order.ASC));
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PageList<Pessoa>>();
   }

   [Then(@"listará (.*) cadastros")]
   public void EntaoListaraCadastros(int quantidade)
   {
      var pessoas = _context["pessoas"] as PageList<Pessoa>;

      pessoas.ShouldNotBeNull();
      pessoas.Items.Count.ShouldBe(quantidade);
   }

   [Then(@"retornará status (.*)")]
   public void EntaoRetornaraStatus(int status)
   {
      var code = (int?)_context["status"];

      code.ShouldNotBeNull();
      code.ShouldBe(status);
   }

   [Then(@"conterá os dados")]
   public void EntaoConteraOsDados(Table table)
   {
      var pessoas = _context["pessoas"] as PageList<Pessoa>;

      pessoas.ShouldNotBeNull();

      for (int i = 0; i < table.Rows.Count; i++)
      {
         var nomeCompleto = table.Rows[i]["nome"];
         var nascimento = table.Rows[i]["nascimento"] ?? "";
         var dataNascimento = nascimento.ToDateOnly("dd/MM/yyyy");

         Console.WriteLine($"db: {pessoas.Items[i].Nome} | tst: {nomeCompleto} ");

         pessoas.Items[i].Nome.ShouldBe(nomeCompleto);
         pessoas.Items[i].Nascimento.ShouldBe(dataNascimento);
      }
   }

   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DropAsync(x => x.Nome == "Sicrano");
   }
}