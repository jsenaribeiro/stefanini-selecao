namespace Api.Service.Controllers;

using MediatR;
using Api.Domain;
using Api.Infrastructure.Values;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using Microsoft.SqlServer.Server;


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

         if (result is PageList pl && pl.Total == 0)
            return NotFound(result);

         if (result is System.Collections.IList list && list.Count == 0)
            return NotFound(result);

         return Ok(result);
      }
      catch (DomainException ex)
      {
         logger.LogError(ex, ex.Message);
         return StatusCode(ex.Status, new DomainError(ex));
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