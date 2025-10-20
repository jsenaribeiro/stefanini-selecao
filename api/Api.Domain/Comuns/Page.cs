namespace Api.Domain;

public record Page
{
   public Page() { }

   public Page(int length, int number)
   {
      this.Length = length == 0 ? 10 : length;
      this.Number = number == 0 ? 1 : number;
   }

   public int Length { get; set; }

   public int Number { get; set; }

   public static Page Default => new(10, 1);
}