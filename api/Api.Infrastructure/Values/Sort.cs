namespace Api.Infrastructure.Values;

public record Sort(string? Field, Order Order = Order.ASC)
{
   public static Sort Default => new(default(string));
}