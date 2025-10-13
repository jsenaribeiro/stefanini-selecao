using Api.Domain;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Repositories;

/// <summary>
/// Base Repository for basic CRUD operations
/// </summary>
public abstract class AbstractRepository<E> : IRepository<E> where E : Entity
{   private readonly DbSet<E> dbSet;

   private readonly SqlContext context;

   public AbstractRepository(IServiceProvider provider)
   {
      context = provider.GetRequiredService<SqlContext>();
      dbSet = context.Set<E>();
   }

   public Task<E?> LoadAsync(Guid id) =>
      dbSet.FirstOrDefaultAsync(x => x.Id == id);

   public Task<E[]> ListAsync() => dbSet.ToArrayAsync();

   public Task<E[]> ListAsync(Expression<Func<E, bool>> predicate) =>
      dbSet.Where(predicate).AsNoTracking().ToArrayAsync();

   public IQueryable<E> Where(Expression<Func<E, bool>> predicate) =>
      dbSet.Where(predicate).AsNoTracking();

   public Task<long> CountAsync() => dbSet.LongCountAsync();

   public Task<long> CountAsync(Expression<Func<E, bool>> predicate) =>
      dbSet.LongCountAsync(predicate);

   public Task<bool> ExistsAsync() => dbSet.AnyAsync();

   public Task<bool> ExistsAsync(Guid id) => dbSet.AnyAsync(x => x.Id == id);

   public Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate) =>
      dbSet.AnyAsync(predicate);
   
   public async Task<E> SaveAsync(E entity)
   {
      try
      {
         if (entity.Id == Guid.Empty)
         {
            entity.Id = Guid.NewGuid();
            entity.DataCriacao = DateTime.Now;
            
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
         }

         entity.DataAtualizacao = DateTime.Now;

         var current = await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);
         if (current is null) throw new Exception("Entidade não encontrada");
         if (current.Equals(entity)) return current;
         if (current.GetType() != entity.GetType()) throw new Exception("Tipo de entidade diferente");
         if (current.Id != entity.Id) throw new Exception("Id de entidade diferente");

         // Atualiza os campos da entidade atual com os valores da entidade recebida
         foreach (var property in typeof(E).GetProperties())
         {
            if (property.CanWrite)
            {
               var newValue = property.GetValue(entity);
               property.SetValue(current, newValue);
            }
         }

         context.Entry(current).State = EntityState.Modified;
         context.Entry(current).OriginalValues.SetValues(entity);

         dbSet.Update(current);
         await context.SaveChangesAsync();
         return current;
      }
      catch (DbUpdateConcurrencyException ex)
      {
         throw new Exception("Erro de concorrência", ex);
      }
      catch (DbUpdateException ex)
      {
         throw new Exception("Erro ao atualizar entidade", ex);
      }
      catch (Exception ex)
      {
         throw new Exception("Erro inesperado", ex);
      }
   }

   public async Task<bool> DropAsync(Guid id)
   {
      try
      {
         var entidade = await dbSet.FirstOrDefaultAsync(x => x.Id == id);

         if (entidade is null) return false;
         else dbSet.Remove(entidade);
         await context.SaveChangesAsync();

         context.ChangeTracker.Clear();

         return true;
      }
      catch (DbUpdateConcurrencyException) { return false; }
      catch (DbUpdateException) { return false; }
      catch (Exception) { return false; }
   }

   public async Task<bool> DropAsync(Expression<Func<E, bool>> predicate)
   {
      var entities = await this.Where(predicate).ToArrayAsync();

      foreach (var entity in entities) await DropAsync(entity.Id);
      
      return true;
   }
}