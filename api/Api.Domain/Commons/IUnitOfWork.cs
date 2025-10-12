using Api.Domain.Pessoas;

namespace Api.Domain;

public interface IUnitOfWork
{
   IPessoaRepository Pessoas { get; }

   Task BeginAsync();

   Task CommitAsync();

   Task RollbackAsync();
}
