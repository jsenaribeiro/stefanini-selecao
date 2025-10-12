using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Api.Domain;

public class EmailValidationAttribute : ValidationAttribute<string?>
{
   public EmailValidationAttribute(bool isRequired) : base(isRequired) { }

   protected override string Validate(string? value, string field, IServiceProvider provider)
   {
      var invalido = string.Format(Messages.INVALIDO, field);
      
      try { new MailAddress(value!); }
      catch { return invalido; }
      return string.Empty;
   }
}