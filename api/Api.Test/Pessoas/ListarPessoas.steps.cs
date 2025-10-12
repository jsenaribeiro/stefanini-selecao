using System.Globalization;
using System.Runtime.InteropServices;
using Api.Domain.Pessoas;
using Api.Service.Contracts;
using Api.Service.Controllers;
using Shouldly;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;

namespace Api.Test.Pessoas;

[Binding]
public class StepDefinitions : AbstractSteps
{
   private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

   private readonly ScenarioContext _context;

   private readonly PessoaController _controller;

   private readonly PageQuery _pageQuery = new(new(10, 1), new("nascimento"));


   public StepDefinitions(ScenarioContext sc)
   {
      _context = sc;
      _context["paginado"] = false;
      _controller = new PessoaController(provider!);
   }

   [Given(@"que os seguintes cadastros")]
   public void DadoQueOsSeguintesCadastros(Table table)
   {
      var awaits = new List<Task>();

      foreach (var row in table.Rows)
      {
         var nome = row["nome"];
         var nascimento = row["nascimento"];
         var dataNascimento = DateOnly.ParseExact(nascimento, "dd/MM/yyyy", _culture);
         var pessoa = new Pessoa(nome, dataNascimento);

         awaits.Add(unitOfWork!.Pessoas.SaveAsync(pessoa));
      }

      Task.WaitAll(awaits.ToArray());
   }

   [When(@"listar os cadastros")]
   public async Task QuandoListarOsCadastros()
   {
      var query = new PessoaQuery(_pageQuery);

      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PagedList<Pessoa>>();
   }

   [When(@"filtra cadastros com ""(.*)""")]
   public async Task QuandoFiltraCadastrosCom(string nome)
   {
      var query = new PessoaQuery(_pageQuery, nome);
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PagedList<Pessoa>>();
   }

   [When(@"listar com (.*) linhas por página")]
   public async Task QuandoListarComLinhasPorPagina(int linhasPorPagina)
   {
      _context["paginado"] = true;
      var pageQuery = new PageQuery(new(linhasPorPagina, 1), _pageQuery.Sort);
      var query = new PessoaQuery(pageQuery);
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PagedList<Pessoa>>();
   }

   [When(@"listar ordenado de modo crescente")]
   public async Task QuandoListarOrdenadoDeModoCrescente()
   {
      var pageQuery = new PageQuery(new(10, 1), new("nome", Order.ASC));
      var query = new PessoaQuery(pageQuery);
      var result = await _controller.Get(query);

      _context["status"] = result.GetStatusCode();
      _context["pessoas"] = result.ValueOf<PagedList<Pessoa>>();
   }

   [Then(@"listará (.*) cadastros")]
   public void EntaoListaraCadastros(int quantidade)
   {
      var pessoas = _context["pessoas"] as PagedList<Pessoa>;

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
      var pessoas = _context["pessoas"] as PagedList<Pessoa>;

      pessoas.ShouldNotBeNull();

      var paginado = (bool)_context["paginado"];

      for (int i = 0; i < table.Rows.Count; i++)
      {
         var nomeCompleto = table.Rows[i]["nome"];
         var nascimento = table.Rows[i]["nascimento"];
         var dataNascimento = DateOnly.ParseExact(nascimento, "dd/MM/yyyy", _culture);

         Console.WriteLine($"db: {pessoas.Items[i].Nome} | tst: {nomeCompleto} ");

         pessoas.Items[i].Nome.ShouldBe(nomeCompleto);
         pessoas.Items[i].Nascimento.ShouldBe(dataNascimento);
      }
   }

   [AfterScenario]
   public async Task ClearScenario()
   {
      await unitOfWork!.Pessoas.DropAsync(x => x.Nome == "Fulano");
      await unitOfWork!.Pessoas.DropAsync(x => x.Nome == "Beltrano");
      await unitOfWork!.Pessoas.DropAsync(x => x.Nome == "Sicrano");
   }
}