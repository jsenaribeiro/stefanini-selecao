namespace Api.Domain;

public record Sort(string? Field, Ordering Order = Ordering.ASC)
{
   public static Sort Default => new(default(string));
}