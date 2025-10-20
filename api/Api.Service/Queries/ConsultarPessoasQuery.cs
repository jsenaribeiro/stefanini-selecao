using MediatR;
using Api.Service.Results;
using Api.Domain;
using Api.Domain.Pessoas;

namespace Api.Service.Queries;

public record ConsultarPessoasQuery : IRequest<PessoaListResult>
{
   public string? Nome { get; set; }

   public Page Page { get; set; } = Page.Default;

   public Sort Sort { get; set; } = Sort.Default;
}