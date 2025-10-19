using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Api.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Ordering { ASC = 0, DESC }

public interface IRepository<E, I>
   : IReadRepository<E, I>
   , IWriteRepository<E, I>
      where E : Entity<I>
      where I : struct
{
   #region defaults

   async Task<bool> DeleteAsync(I id) =>
      await DeleteAsync(await LoadAsync(id));

   async Task<bool> DeleteAsync(Expression<Func<E, bool>> predicate)
   {
      var returns = new List<bool>();

      var founds = await Where(predicate).ListAsync(true);

      foreach (var entity in founds)
         returns.Add(await DeleteAsync(entity.Id));

      return returns.All(x => x);
   }

   #endregion
}

public interface IWriteRepository<E, I> where E : Entity<I> where I : struct
{
   Task<E> CreateAsync(E? entity);

   Task<E> UpdateAsync(E? entity);

   Task<bool> DeleteAsync(E? entity);
}

public interface IReadRepository<E, I> where E : Entity<I> where I : struct
{
   Task<bool> ExistsAsync();

   Task<long> CountAsync();

   Task<E?> LoadAsync(I Id);

   Task<E?> LoadAsync();

   Task<E[]> ListAsync(bool isReadOnly);

   Task<T[]> ListAsync<T>(bool isReadOnly, Expression<Func<E, T>> selector);

   Task<(E[] Items, int Total)> ListAsync(int number, int length);

   IReadRepository<E, I> OrderBy(string? field, Ordering order);

   IReadRepository<E, I> Where(Expression<Func<E, bool>> predicate);

   #region defaults

   Task<E[]> ListAsync() => ListAsync(false);

   Task<T[]> ListAsync<T>(Expression<Func<E, T>> selector) => ListAsync(false, selector);

   Task<bool> ExistsAsync(I id) => Where(x => x.Id.Equals(id)).ExistsAsync();

   Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate) =>
      Where(predicate).ExistsAsync();

   #endregion
}