using Microsoft.Extensions.DependencyInjection;
using Api.Domain.Pessoas;
using Api.Domain;
using Api.Infrastructure.Repositories;

namespace Api.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
   private IServiceProvider _provider;

   public UnitOfWork(IServiceProvider provider)
   {
      _provider = provider;
      Pessoas = new PessoaRepository(provider);
   }

   public IPessoaRepository Pessoas { get; }

   public Task BeginAsync() => throw new NotImplementedException();

   public Task CommitAsync() => throw new NotImplementedException();

   public Task RollbackAsync() => throw new NotImplementedException();
}
