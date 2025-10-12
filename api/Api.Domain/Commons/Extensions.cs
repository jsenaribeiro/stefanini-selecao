using System.Globalization;
using Api.Domain;

public static class Extensions
{
   private static CultureInfo _culture = CultureInfo.InvariantCulture;

   public static DateOnly ToDateOnly(this string value, string format) =>
      TryExact(() => DateOnly.ParseExact(value, format, _culture),
               () => DateOnly.Parse(value));

   public static TimeOnly ToTimeOnly(this string value, string format) =>
      TryExact(() => TimeOnly.ParseExact(value, format, _culture),
               () => TimeOnly.Parse(value));

   public static DateTime ToDateTime(this string value, string format) =>
      TryExact(() => DateTime.ParseExact(value, format, _culture, DateTimeStyles.None),
               () => DateTime.Parse(value));

   private static T TryExact<T>(Func<T> try1, Func<T> try2)
   {
      var error = new InvalidCastException(typeof(T).Name);

      try { return try1(); }
      catch
      {
         try { return try2(); }
         catch { throw error; }
      }
   }
}