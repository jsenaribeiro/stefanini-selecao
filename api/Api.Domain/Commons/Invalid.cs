using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Api.Domain;

public record Invalid(string field, object? value, string error)
{
   public static bool Of<T>(T instance, IServiceProvider provider, out Invalid[] invalids) where T : notnull
   {
      var results = new List<ValidationResult>();
      var context = new ValidationContext(instance!, provider, null);
      var isValid = Validator.TryValidateObject(instance!, context, results, validateAllProperties: true);
      var fields = results.SelectMany(x => x.MemberNames.Select(n => new { field = n, error = x.ErrorMessage }));
      var errors = fields.Select(item =>
      {
         var typed = instance.GetType();
         var flags = BindingFlags.Public | BindingFlags.Instance;
         var props = typed?.GetProperty(item.field, flags)?.GetValue(instance);
         var error = string.Format(Messages.INVALIDO, item.field);

         return new Invalid(item.field, props, item.error ?? error);
      });

      invalids = errors.ToArray();

      return isValid;
   }

   public static void ThrowIfInvalid<T>(T instance, IServiceProvider provider) where T : notnull
   {
      var isValid = Of(instance, provider, out var invalids);
      if (!isValid) throw Failure.Invalid(invalids);
   }
}