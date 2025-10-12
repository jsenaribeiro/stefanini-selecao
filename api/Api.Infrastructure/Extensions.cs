using Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Order { ASC = 0, DESC }

public record Page(int Size, int Number);

public record Sort(string? Field, Order Order = Order.ASC);

public record PageQuery(Page Page, Sort Sort)
{
   public PageQuery() : this(Default) { }

   public PageQuery(Page page) : this(page, new(null)) { }

   public static PageQuery Default => new PageQuery(new(10, 1), new(null));
}

public static class QueryableExtensions
{
   private static IQueryable<E> GetQueryPage<E>(IQueryable<E> queryable, PageQuery query) where E : Entity
   {
      var (sort, page) = (query.Sort, query.Page);
      var pageNumber = page.Number == 0 ? 1 : page.Number;
      var pageSkip = (pageNumber - 1) * page.Size;
      var ordered = string.IsNullOrWhiteSpace(sort.Field) ? queryable
         : sort.Order == Order.ASC ? queryable.OrderBy(sort.Field)
         : queryable.OrderBy($"{sort.Field} descending");

      var paginated = ordered.Skip(pageSkip).Take(page.Size);

      return paginated;
   }

   public static (List<E> items, int total) ToPagedList<E>(this IQueryable<E> queryable, PageQuery query) where E : Entity
   {
      var items = GetQueryPage(queryable, query).ToList();
      var total = queryable.Count();

      return (items, total);
   }

   public static async Task<(List<E> items, int total)> ToPagedListAsync<E>(this IQueryable<E> queryable, PageQuery query) where E : Entity
   {
      var items = await GetQueryPage(queryable, query).ToListAsync();
      var total = await queryable.CountAsync();

      return (items, total);
   }
}