using Api.Domain.Pessoas;
using Api.Infrastructure.Values;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Queries;

public record ConsultarPessoasQuery : IRequest<PageList<PessoaResult>>
{
   public string? Nome { get; set; }

   public Page Page { get; set; } = Page.Default;

   public Sort Sort { get; set; } = Sort.Default;
}