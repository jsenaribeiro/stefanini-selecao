namespace Api.Infrastructure.Values;

public record Page(int Length, int Number)
{
   public static Page Default => new(10, 1);
}