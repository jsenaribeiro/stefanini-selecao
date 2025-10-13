using Api.Domain.Pessoas;
using Api.Infrastructure.Values;

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
      Nascimento = pessoa.Nascimento.ToString("dd/MM/yyyy");
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

   public static PageList<PessoaResult> From(int total, params Pessoa[] pessoas) =>
      new PageList<PessoaResult>(From(pessoas), total);
}