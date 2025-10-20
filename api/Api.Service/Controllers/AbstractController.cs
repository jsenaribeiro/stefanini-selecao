namespace Api.Service.Controllers;

using MediatR;
using Api.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using Api.Service.Results;
using System.Text.Json;


public abstract class AbstractController<E> : ControllerBase where E : class
{
   protected readonly ILogger<E> logger;

   protected readonly IMediator mediator;

   public AbstractController(IServiceProvider provider)
   {
      logger = provider.GetRequiredService<ILogger<E>>();
      mediator = provider.GetRequiredService<IMediator>();
   }

   protected async Task<IActionResult> SendAsync<T>(IRequest<T> request, bool isCreation = false)
   {
      try
      {
         var result = await mediator.Send(request);

         if (result is PageList pl && pl.Total == 0)
            return NotFound(result);

         if (result is System.Collections.IList list && list.Count == 0)
            return NotFound(result);

         return isCreation ? StatusCode(201, result) : Ok(result);
      }
      catch (JsonException ex)
      {
         logger.LogError(ex, ex.Message);

         var field = ex.Path?.Replace("$.", "") ?? "";

         var value = request.GetType().GetProperties()
            .First(p => p.Name.ToLower() == field.ToLower())
            .GetValue(request);

         var error = string.Format(Messages.INVALIDO, field);

         return StatusCode(400, ErrorResult.From(field, error, value));
      }
      catch (DomainException ex)
      {
         logger.LogError(ex, ex.Message);
         return StatusCode(ex.Status, ErrorResult.From(ex));
      }
      catch (AuthenticationException ex)
      {
         logger.LogError(ex, ex.Message);
         return Unauthorized(ex.Message);
      }
      catch (ArgumentException ex)
      {
         logger.LogError(ex, ex.Message);
         return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "Erro inesperado");
         return StatusCode(500, "Erro inesperado");
      }
   }
}