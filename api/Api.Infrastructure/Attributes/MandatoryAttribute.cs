using System.ComponentModel.DataAnnotations;
using Api.Domain;

namespace Api.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MandatoryAttribute : ValidationAttribute
{
   protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
   {
      var property = validationContext.MemberName ?? "NAO_IDENTIFICADO";

      var isEmpty = value is null || value.ToString() == "" 
         || value is DateTime dt && dt == default
         || value is DateOnly d && d == default
         || value is TimeOnly t && t == default;

      var success = ValidationResult.Success!;

      var required = string.Format(Messages.OBRIGATORIO, property);

      ErrorMessage = isEmpty ? required : string.Empty;

      return ErrorMessage == string.Empty ? success
           : new ValidationResult(ErrorMessage, [ property ]);
   }
}

