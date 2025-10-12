using MediatR;
using Api.Domain;
using Api.Domain.Pessoas;
using Api.Service.Contracts;

namespace Api.Service.Handlers;

public class PessoaHandler : AbstractHandler
   , IRequestHandler<PessoaQuery, PagedList<Pessoa>>
{
   public PessoaHandler(IServiceProvider provider) : base(provider) { }

   public async Task<PagedList<Pessoa>> Handle(PessoaQuery query, CancellationToken cancel)
   {
      if (query is null) throw new ArgumentNullException(nameof(PessoaQuery));

      var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
      var nome = string.IsNullOrWhiteSpace(query.Nome) ? null : query.Nome.ToLower();

      var exp = from p in unitOfWork.Pessoas.Query
                where nome == null || p.Nome.ToLower().Contains(nome)
                select p;

      var (items, total) = await exp.ToPagedListAsync(query);

      return new PagedList<Pessoa>(items, total);
   }
}