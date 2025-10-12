using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Domain.Pessoas;
using Api.Service.Contracts;

namespace Api.Service.Controllers;

using AsyncResult = Task<IActionResult>;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ApiController<Pessoa>
{
   public PessoaController(IServiceProvider provider) : base(provider) { }

   /// <summary>
   /// Consultar pessoas
   /// </summary>
   [AllowAnonymous]
   [HttpGet]
   public AsyncResult Get([FromQuery] PessoaQuery query) =>
      TryAsync(() => mediator.Send(query));
}