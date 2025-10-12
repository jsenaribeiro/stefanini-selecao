using Api.Domain.Pessoas;
using Api.Infrastructure.Values;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Queries;

public record ConsultarPessoasQuery(Page Page, Sort Sort, string Nome) : IRequest<PageList<PessoaResult>>
{
   public ConsultarPessoasQuery(Page page, Sort sort) : this(page, sort, string.Empty) { }

   public ConsultarPessoasQuery(Sort sort, string nome) : this(Page.Default, sort, nome) { }

   public ConsultarPessoasQuery(string nome) : this(Page.Default, Sort.Default, nome) { }

   public ConsultarPessoasQuery(Page page) : this(page, Sort.Default, string.Empty) { }

   public ConsultarPessoasQuery(Sort sort) : this(Page.Default, sort, string.Empty) { }

   public ConsultarPessoasQuery() : this(Page.Default, Sort.Default, string.Empty) { }
}