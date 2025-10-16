using System.Text.RegularExpressions;
using Api.Domain;

public class CpfValidatorAttribute : ValidatorAttribute<string>
{
   protected override string Validate(string value, string field, IServiceProvider provider)
   {
      if (isValidCpfNumber(value) == false)
         return string.Format(Messages.INVALIDO, field);

      return string.Empty;
   }

   private bool isValidCpfNumber(string cpf)
   {
      string d = Regex.Replace(cpf ?? string.Empty, @"[^\d]", "");

      if (d.Length != 11 || d.All(c => c == d.First()))
         return false;

      int[] digitos = d.Select(c => (int)char.GetNumericValue(c)).ToArray();

      int resto1 = digitos.Take(9)
          .Select((digito, i) => digito * (10 - i))
          .Sum() % 11;

      int dv1 = resto1 < 2 ? 0 : 11 - resto1;

      if (digitos[9] != dv1)
         return false;

      int resto2 = digitos.Take(10)
          .Select((digito, i) => digito * (11 - i))
          .Sum() % 11;

      int dv2 = resto2 < 2 ? 0 : 11 - resto2;

      return digitos[10] == dv2;
   }
}