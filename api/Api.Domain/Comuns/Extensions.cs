using System.Globalization;
using Api.Domain;

public static class Extensions
{
   private static CultureInfo _culture = CultureInfo.InvariantCulture;

   public static DateOnly ToDateOnly(this string value, string format) =>
      TryExact(() => DateOnly.ParseExact(value, format, _culture),
               () => DateOnly.Parse(value), value);

   public static TimeOnly ToTimeOnly(this string value, string format) =>
      TryExact(() => TimeOnly.ParseExact(value, format, _culture),
               () => TimeOnly.Parse(value), value);

   public static DateTime ToDateTime(this string value, string format) =>
      TryExact(() => DateTime.ParseExact(value, format, _culture, DateTimeStyles.None),
               () => DateTime.Parse(value), value);

   private static T TryExact<T>(Func<T> converter, Func<T> parser, string value)
   {
      var fail = $"Falha converter '{value}' para {typeof(T).Name}";
      var exception = new InvalidOperationException(fail);

      try { return converter(); }
      catch
      {
         try { return parser(); }
         catch { throw exception; }
      }
   }
}