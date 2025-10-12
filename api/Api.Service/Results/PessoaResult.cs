using Api.Domain.Pessoas;
using Api.Infrastructure.Values;

namespace Api.Service.Results;

public record PessoaResult
(
   Guid Id,
   char Sexo,
   string? Cpf,
   string Nome,
   string? Email,
   string Nascimento,
   string? Nacionalidade
)
{
   public PessoaResult() : this(Guid.Empty, ' ', null, "", null, "", null) { }

   public PessoaResult(Pessoa pessoa): this(From(pessoa)) { }

   public static PessoaResult From(Pessoa pessoa) => new PessoaResult
   (
      pessoa.Id,
      (pessoa.Sexo?.ToString() ?? " ")[0],
      pessoa.CPF,
      pessoa.Nome,
      pessoa.Email,
      pessoa.Nascimento.ToString("dd/MM/yyyy"),
      pessoa.Nacionalidade
   );

   public static PessoaResult[] From(params Pessoa[] pessoas) =>
      pessoas.Select(From).ToArray();

   public static PageList<PessoaResult> From(int total, params Pessoa[] pessoas) =>
      new PageList<PessoaResult>(From(pessoas).ToList(), total);
}