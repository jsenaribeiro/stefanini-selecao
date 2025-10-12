using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Api.Domain;

public interface IReadRepository<E> where E : Entity
{
   IQueryable<E> Query { get; } // montando no handler

   Task<bool> ExistsAsync { get; }

   Task<long> CountAsync { get; }
}