using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Domain.Pessoas;
using Api.Service.Queries;
using Api.Service.Commands;


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
   [HttpGet]
   [AllowAnonymous]
   public AsyncResult Get([FromQuery] ConsultarPessoasQuery query) =>
      TryAsync(() => mediator.Send(query));

   /// <summary>
   /// Cadastrar pessoa
   /// </summary>
   /// <returns>Pessoa cadastrar com Id preenchido</returns>
   [HttpPost]
   public AsyncResult Post([FromBody] CadastrarPessoaCommand command) =>
      TryAsync(() => mediator.Send(command), true);
}