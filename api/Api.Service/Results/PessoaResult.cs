using Api.Domain;
using Api.Domain.Pessoas;
using Api.Service.Queries;

namespace Api.Service.Results;

public record PessoaResult
{
   public PessoaResult(Pessoa pessoa)
   {
      Id = pessoa.Id;
      Cpf = pessoa.CPF;
      Sexo = pessoa.Sexo?.ToString()[0] ?? ' ';
      Nome = pessoa.Nome;
      Email = pessoa.Email;
      Nascimento = pessoa.Nascimento.ToString("yyyy-MM-dd");
      Nacionalidade = pessoa.Nacionalidade;
   }

   public Guid Id { get; set; }
   public char Sexo { get; set; }
   public string? Cpf { get; set; }
   public string Nome { get; set; }
   public string? Email { get; set; }
   public string Nascimento { get; set; }
   public string? Nacionalidade { get; set; }

   public static PessoaResult[] From(params Pessoa[] pessoas) =>
      pessoas.Select(p => new PessoaResult(p)).ToArray();
}

public record PessoaListResult : PageListResult<PessoaResult>
{
   public PessoaListResult(int sum, int size, int pages, int number, PessoaResult[] records)
      : base(sum, size, pages, number, records) { }

   public PessoaListResult(PageList<Pessoa> pessoas, Page pagina) : this(0, 0, 0, 0, [])
   {
      var quantidadePaginas = (pessoas.Total / pagina.Length) + 1;
      var registros = pessoas.Items.Select(p => new PessoaResult(p)).ToArray();

      sum = pessoas.Total;
      size = pagina.Length;
      pages = quantidadePaginas;
      number = pagina.Number;
      records = registros;
   }
}
