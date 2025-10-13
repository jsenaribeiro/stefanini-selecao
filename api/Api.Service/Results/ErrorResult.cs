using Api.Domain;

namespace Api.Service.Results;

public record ErrorResult(string Message, Invalid[] Invalids)
{
   public static ErrorResult From(DomainException ex) =>
      new ErrorResult(ex.Message, ex.Invalids);
};