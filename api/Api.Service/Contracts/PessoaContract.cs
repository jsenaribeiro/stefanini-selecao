using Api.Domain;
using Api.Domain.Pessoas;
using MediatR;

namespace Api.Service.Contracts;

public record PessoaQuery(PageQuery query, string Nome) : PageQuery(query), IRequest<PagedList<Pessoa>>
{
   public PessoaQuery(string nome) : this(Default, nome) => this.Nome = nome;

   public PessoaQuery(PageQuery query) : this(query, string.Empty) { }

   public PessoaQuery() : this(Default, "") { }
}

public record PessoaResult
(
   char cpf,
   string nome,
   string email,
   string nascimento,
   string nacionalidade
);