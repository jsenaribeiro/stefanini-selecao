namespace Api.Infrastructure.Values;

public record Page(int Size, int Number)
{
   public static Page Default => new(10, 1);
}