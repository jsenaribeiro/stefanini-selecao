using System.Text.Json;
using Api.Domain;
using Api.Service.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Service.Middlers;

public class JsonExceptionFilter : IExceptionFilter
{
   private readonly ILogger<JsonException> _logger;

   public JsonExceptionFilter(ILogger<JsonException> logger) => _logger = logger;

   public void OnException(ExceptionContext context)
   {
      if (context.Exception is JsonException ex)
      {
         _logger.LogError(ex, ex.Message);

         var field = ex.Path?.Replace("$.", "") ?? "";
         var error = string.Format(Messages.INVALIDO, field);
         var value = ErrorResult.From(field, error);

         context.Result = new BadRequestObjectResult(value);
      }

      context.ExceptionHandled = true;
   }
}