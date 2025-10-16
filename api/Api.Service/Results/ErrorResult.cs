using Api.Domain;

namespace Api.Service.Results;

public record ErrorResult(string Message, Invalid[] Invalids)
{
   public static ErrorResult From(DomainException ex) =>
      new ErrorResult(ex.Message, ex.Invalids);

   public static ErrorResult From(string field, string error, object? value) =>
      new ErrorResult(error, new[] { new Invalid(field, value, error) });

   public static ErrorResult From(string field, string error) =>
      new ErrorResult(error, new[] { new Invalid(field, null, error) });
};