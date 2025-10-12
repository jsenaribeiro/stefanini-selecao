using System.Linq.Expressions;

namespace Api.Domain;

public interface IWriteRepository<E> where E : Entity
{
   Task<E> SaveAsync(E entity);

   Task<bool> DropAsync(Guid id);

   Task<bool> DropAsync(Expression<Func<E, bool>> predicate);
}
