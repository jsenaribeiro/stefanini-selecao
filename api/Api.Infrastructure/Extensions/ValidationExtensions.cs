using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Api.Domain;

public static class ValidationExtensions
{
   public static bool ValitateOf<T>(this ValidationContext context, bool all, out Invalid[] invalids)
   {
      var instance = context.ObjectInstance;
      var results = new List<ValidationResult>();
      var isValid = Validator.TryValidateObject(instance!, context, results, validateAllProperties: all);
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
}