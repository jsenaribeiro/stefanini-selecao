using System;
using System.ComponentModel.DataAnnotations;
using Api.Domain;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public abstract class ValidatorAttribute<T> : ValidationAttribute
{
   protected abstract string Validate(T value, string field, IServiceProvider provider);

   protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
   {
      var provider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;

      var property = validationContext.MemberName ?? "NAO_IDENTIFICADO";

      if (provider is null) throw new InvalidCastException(Messages.SEM_PROVIDER);

      var isEmpty = value is null || EqualityComparer<T>.Default.Equals((T)value, default);

      var required = string.Format(Messages.OBRIGATORIO, property);

      var success = ValidationResult.Success!;

      ErrorMessage = isEmpty || value is null ? "" : Validate((T)value, property, provider);

      return ErrorMessage == string.Empty ? success
           : new ValidationResult(ErrorMessage, [ property ]);
   }
}

