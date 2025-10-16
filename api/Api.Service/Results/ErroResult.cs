
using Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Service.Results;

public class ErroResult : ObjectResult
{
   public ErroResult(int statusCode, object? value) : base(value)
   {
      this.StatusCode = statusCode;
   }

   public static ErroResult From(DomainException e) =>
      new ErroResult(e.Status, e.Data);
}