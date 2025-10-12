namespace Api.Service.Controllers;

using Api.Domain;
using Api.Service.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

public abstract class ApiController<E> : ControllerBase where E : class
{
   protected readonly ILogger<E> logger;

   protected readonly IMediator mediator;

   public ApiController(IServiceProvider provider)
   {
      logger = provider.GetRequiredService<ILogger<E>>();
      mediator = provider.GetRequiredService<IMediator>();
   }

   protected async Task<IActionResult> TryAsync<T>(Func<Task<T>> task)
   {
      try
      {
         var result = await task();

         if (result is PagedList pl && pl.Total == 0)
            return NotFound(result);

         if (result is System.Collections.IList list && list.Count == 0)
            return NotFound(result);

         return Ok(result);
      }
      catch (DomainException ex)
      {
         logger.LogError(ex, ex.Message);
         return StatusCode(ex.Status, ex.Message);
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