using System.Net.Mail;

namespace Api.Domain.Pessoas;

public record Email(string email)
{
   public static Email Empty = new Email("");

   public bool IsValid
   {
      get
      {
         if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            MailAddress mailAddress = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
      }
   }
}