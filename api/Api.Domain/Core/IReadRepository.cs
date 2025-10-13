using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Api.Domain;

public interface IReadRepository<E> where E : Entity
{
   Task<E?> LoadAsync(Guid id);

   Task<E[]> ListAsync();

   Task<E[]> ListAsync(Expression<Func<E, bool>> predicate);

   IQueryable<E> Where(Expression<Func<E, bool>> predicate);

   Task<bool> ExistsAsync();

   Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate);

   Task<long> CountAsync(Expression<Func<E, bool>> predicate);

   Task<long> CountAsync();
}