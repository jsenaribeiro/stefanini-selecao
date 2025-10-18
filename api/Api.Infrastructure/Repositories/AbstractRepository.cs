using Api.Domain;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Data.SqlClient;

namespace Api.Infrastructure.Repositories;

/// <summary>
/// Base Repository for basic CRUD operations
/// </summary>
public abstract class AbstractRepository<E, I> : IRepository<E, I>
   where E : Entity<I> where I : struct
{
   protected readonly SqlContext _context;

   protected DbSet<E> _contextSet => _context.Set<E>();

   protected readonly ILogger<IRepository<E, I>> _logger;

   private (int Size, int Number) _page = (0, 0);

   private (string? Field, Ordering Order) _sort = (null, Ordering.ASC);

   private IQueryable<E> _query;

   public AbstractRepository(IServiceProvider provider)
   {
      _context = provider.GetRequiredService<SqlContext>();
      _logger = provider.GetRequiredService<ILogger<IRepository<E, I>>>();
      _query = _contextSet;
   }

   public IReadRepository<E, I> PageBy(int size, int number) =>
      fluentOf(() => _page = (size, number));

   public IReadRepository<E, I> OrderBy(string? field, Ordering order) =>
      fluentOf(() => _sort = (field, order));

   public IReadRepository<E, I> FilterBy(Expression<Func<E, bool>> predicate) =>
      fluentOf(() => _query = _query.Where(predicate));

   public Task<E?> LoadAsync() => _query.FirstOrDefaultAsync();

   public Task<E?> LoadAsync(I id) => _contextSet
      .AsNoTrackingWithIdentityResolution()
      .FirstOrDefaultAsync(x => x.Id.Equals(id));

   public Task<E[]> ListAsync(Expression<Func<E, bool>> predicate) =>
      _contextSet.Where(predicate).AsNoTracking().ToArrayAsync();

   public Task<bool> ExistsAsync() => _query.AnyAsync();

   public Task<long> CountAsync() => _query.LongCountAsync();   

   public async Task<(E[] Items, int Total)> ListAsync()
   {
      _query ??= _contextSet.AsNoTracking();

      var pageNumber = _page.Number == 0 ? 1 : _page.Number;
      var pageSkip = (pageNumber - 1) * _page.Size;
      var (field, order) = _sort;
      
      var ordered = string.IsNullOrWhiteSpace(field) ? _query
         : order == Ordering.ASC ? _query.OrderBy(field)
         : _query.OrderBy($"{field} descending");

      var paging =_page.Size > 0 && _page.Number > 0
         ? ordered.Skip(pageSkip).Take(_page.Size)
         : ordered;

      var items = await paging.ToArrayAsync();
      var total = await _query.CountAsync();

      _page = (0, 0);
      _sort = (null, Ordering.ASC);
      _query = _contextSet;

      return new(items, total);
   }

   public Task<E> CreateAsync(E? entity) => TryAsync(async () =>
   {
      ArgumentNullException.ThrowIfNull(entity);

      await ValidateUniqueAttributeAsync(entity);

      if (entity.Id is Guid id && id == Guid.Empty)
         entity.Id = (I)(dynamic)Guid.NewGuid();

      entity.Log = Audit.Criacao;

      await _contextSet.AddAsync(entity);
      await _context.SaveChangesAsync();

      return entity;
   });

   public Task<E> UpdateAsync(E? entity) => TryAsync(async () =>
   {
      ArgumentNullException.ThrowIfNull(entity);

      await ValidateUniqueAttributeAsync(entity);

      entity.Log = Audit.Atualizacao;

      if (getTrackedOf(entity.Id) is E tracked)
         _context.Entry(tracked).State = EntityState.Detached;

      _contextSet.Update(entity);

      await _context.SaveChangesAsync();

      return entity;
   });

   public async Task<bool> DeleteAsync(E? entity)
   {
      ArgumentNullException.ThrowIfNull(entity);

      try
      {
         var tracked = getTrackedOf(entity.Id);
         _contextSet.Remove(tracked ?? entity);

         await _context.SaveChangesAsync();
         return true;
      }
      catch(Exception ex)
      {
         _logger.LogError(ex, ex.Message);
         return false;
      }
   }

   private Task<E> TryAsync(Func<Task<E>> callback)
   {
      try
      {
         return callback();
      }
      catch (DbUpdateConcurrencyException ex)
      {
         _logger.LogError(ex, ex.Message);
         
         throw new DomainException(500, "Conflito no banco de dados");
      }
      catch (DbUpdateException ex)
      {
         _logger.LogError(ex, ex.Message);

         if (ex.InnerException is SqlException sqlException)
         {
            var codigoErroDb = sqlException.Number;
            var errosDeDuplicidade = new[] { 2601, 2627 };

            if (errosDeDuplicidade.Contains(codigoErroDb))
               throw new DomainException(400, "Violação de campo único");
         }
         
         throw;
      }
      catch (ArgumentNullException)
      {
         throw;
      }
      catch (DomainException)
      {
         throw;
      }
      catch (Exception ex)
      {
         _logger.LogCritical(ex, ex.Message);
         throw new Exception("Erro inesperado", ex);
      }
   }

   private async Task ValidateUniqueAttributeAsync(E entity)
   {
      var isUniqueAttribute = (CustomAttributeData a) => a.AttributeType.Name == nameof(UniqueAttribute);
      var hasUniqueAttribute = (PropertyInfo p) => p.CustomAttributes.Any(isUniqueAttribute);

      var uniqueProps = entity
         .GetType()
         .GetProperties()
         .Where(hasUniqueAttribute);

      foreach (var uniqueProp in uniqueProps)
      {
         var field = uniqueProp.Name;
         var value = uniqueProp.GetValue(entity);
         var param = Expression.Parameter(typeof(E), "x");
         var props = Expression.Property(param, field);
         var body = Expression.Equal(props, Expression.Constant(value));
         var lambda = Expression.Lambda(body, param) as Expression<Func<E, bool>>;
         var found = await _contextSet.FirstOrDefaultAsync(lambda!);
         var unique = found is null || found.Id.Equals(entity.Id);
         var emptyId = entity.Id.Equals(Guid.Empty);

         if (unique || emptyId) continue;

         var error = string.Format(Messages.DUPLICIDADE, field, value);

         throw new InvalidException(field, error, value);
      }
   }

   private E? getTrackedOf(I id) =>
      _context.ChangeTracker.Entries<E>()
         .FirstOrDefault(e => e.Entity.Id.Equals(id))?.Entity;

   private IReadRepository<E, I> fluentOf(Action action) { action(); return this; }
}