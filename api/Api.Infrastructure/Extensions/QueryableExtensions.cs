using Api.Domain;
using Api.Infrastructure.Values;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Json.Serialization;

public static class QueryableExtensions
{
   public static PageList<E> ToPageList<E>(this IQueryable<E> query)
      where E : Entity => ToPagedList(query, Page.Default, Sort.Default);

   public static PageList<E> ToPageList<E>(this IQueryable<E> query, Page page)
      where E : Entity => ToPagedList(query, page, Sort.Default);

   public static PageList<E> ToPagedList<E>(this IQueryable<E> query, Page page, Sort sort) where E : Entity
   {
      var (items, total) = ToPageListAsync(query, page, sort).Result;

      return new(items, total);
   }

   public static Task<PageList<E>> ToPageListAsync<E>(this IQueryable<E> query) 
      where E : Entity => ToPageListAsync(query, Page.Default, Sort.Default);

   public static Task<PageList<E>> ToPageListAsync<E>(this IQueryable<E> query, Page page) 
      where E : Entity => ToPageListAsync(query, page, Sort.Default);

   public static async Task<PageList<E>> ToPageListAsync<E>(this IQueryable<E> query, Page page, Sort sort) where E : Entity
   {
      var pageNumber = page.Number == 0 ? 1 : page.Number;
      var pageSkip = (pageNumber - 1) * page.Size;
      var ordered = string.IsNullOrWhiteSpace(sort.Field) ? query
         : sort.Order == Order.ASC ? query.OrderBy(sort.Field)
         : query.OrderBy($"{sort.Field} descending");
      var paged = ordered.Skip(pageSkip).Take(page.Size);

      var items = await paged.ToArrayAsync();
      var total = await query.CountAsync();

      return new(items, total);
   }
}