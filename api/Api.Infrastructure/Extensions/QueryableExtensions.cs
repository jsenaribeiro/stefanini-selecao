using Api.Domain;
using Api.Infrastructure.Values;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Json.Serialization;

public static class QueryableExtensions
{
   public static PageList<E> ToPageList<E, I>(this IQueryable<E> query)
      where E : Entity<I> where I : struct =>
         ToPagedList<E, I>(query, Page.Default, Sort.Default);

   public static PageList<E> ToPageList<E, I>(this IQueryable<E> query, Page page)
      where E : Entity<I> where I : struct =>
         ToPagedList<E, I>(query, page, Sort.Default);

   public static PageList<E> ToPagedList<E, I>(this IQueryable<E> query, Page page, Sort sort)
      where E : Entity<I> where I : struct
   {
      var (items, total) = ToPageListAsync<E, I>(query, page, sort).Result;

      return new(items, total);
   }

   public static Task<PageList<E>> ToPageListAsync<E, I>(this IQueryable<E> query)
      where E : Entity<I> where I : struct =>
         ToPageListAsync<E, I>(query, Page.Default, Sort.Default);

   public static Task<PageList<E>> ToPageListAsync<E, I>(this IQueryable<E> query, Page page)
      where E : Entity<I> where I : struct =>
         ToPageListAsync<E, I>(query, page, Sort.Default);

   public static async Task<PageList<E>> ToPageListAsync<E, I>(this IQueryable<E> query, Page page, Sort sort)
      where E : Entity<I> where I : struct
   {
      var pageNumber = page.Number == 0 ? 1 : page.Number;
      var pageSkip = (pageNumber - 1) * page.Length;
      var ordered = string.IsNullOrWhiteSpace(sort.Field) ? query
         : sort.Order == Ordering.ASC ? query.OrderBy(sort.Field)
         : query.OrderBy($"{sort.Field} descending");
      var paged = ordered.Skip(pageSkip).Take(page.Length);

      var items = await paged.ToArrayAsync();
      var total = await query.CountAsync();

      return new(items, total);
   }
}