using Api.Domain.Pessoas;
using Api.Infrastructure.Values;
using Api.Service.Controllers;
using TechTalk.SpecFlow;
using Shouldly;
using Api.Service.Queries;
using Api.Service.Results;
using Api.Domain;

namespace Api.Test.Pessoas;

using PageListResult = PageList<PessoaResult>;

[Binding]
public class ListarPessoasSteps : AbstractSteps
{
   private readonly ScenarioContext _context;

   private readonly PessoaController _controller;

   private readonly ConsultarPessoasQuery _queryDefault;

   public ListarPessoasSteps(ScenarioContext sc)
   {
      _context = sc;
      _context["paginado"] = false;
      _controller = new PessoaController(provider!);
      _queryDefault = new ConsultarPessoasQuery
      {
         Sort = new("nascimento", Ordering.ASC)
      };
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

         await unitOfWork!.Pessoas.CreateAsync(pessoa);
      }
   }

   [When(@"listar os cadastros")]
   public async Task QuandoListarOsCadastros()
   {
      var query = _queryDefault;
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["result"] = result.ValueOf<PageListResult>();
   }

   [When(@"filtra cadastros com ""(.*)""")]
   public async Task QuandoFiltraCadastrosCom(string nome)
   {
      var query = _queryDefault with { Nome = nome };
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["result"] = result.ValueOf<PageListResult>();
   }

   [When(@"listar com (.*) linhas por página")]
   public async Task QuandoListarComLinhasPorPagina(int linhasPorPagina)
   {
      _context["paginado"] = true;

      var query = _queryDefault with { Page = new(linhasPorPagina, 1) };
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["result"] = result.ValueOf<PageListResult>();
   }

   [When(@"listar ordenado de modo crescente")]
   public async Task QuandoListarOrdenadoDeModoCrescente()
   {
      var query = _queryDefault with { Sort = new Sort("nome", Ordering.ASC) };
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["result"] = result.ValueOf<PageListResult>();
   }

   [Then(@"listará (.*) cadastros")]
   public void EntaoListaraCadastros(int quantidade)
   {
      var pessoas = _context.Get<PageListResult>("result");

      pessoas.ShouldNotBeNull();
      pessoas.Items.Length.ShouldBe(quantidade);
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
      var pessoas = _context.Get<PageListResult>("result");

      pessoas.ShouldNotBeNull();

      for (int i = 0; i < table.Rows.Count; i++)
      {
         var nomeCompleto = table.Rows[i]["nome"];
         var nascimento = table.Rows[i]["nascimento"] ?? "";

         Console.WriteLine($"db: {pessoas.Items[i].Nome} | tst: {nomeCompleto} ");

         pessoas.Items[i].Nome.ShouldBe(nomeCompleto);
         pessoas.Items[i].Nascimento.ShouldBe(nascimento);
      }
   }

   protected override async Task ClearScenario()
   {
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Fulano");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Beltrano");
      await unitOfWork.Pessoas.DeleteAsync(x => x.Nome == "Sicrano");
   }
}