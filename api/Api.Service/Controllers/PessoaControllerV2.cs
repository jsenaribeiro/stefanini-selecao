using Api.Domain.Pessoas;
using Api.Service.Queries;
using Api.Service.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Service.Controllers;

using AsyncResult = Task<IActionResult>;

[ApiController]
[ApiVersion("2.0")]
[Route("api/[controller]s")]
public class PessoaControllerV2 : AbstractController<Pessoa>
{
   public PessoaControllerV2(IServiceProvider provider) : base(provider) { }

   /// <summary>
   /// Consultar pessoas
   /// </summary>
   [HttpGet]
   public AsyncResult Get([FromQuery] ConsultarPessoasQuery query) => SendAsync(query);

   /// <summary>
   /// Cadastrar pessoa
   /// </summary>
   /// <returns>Pessoa cadastrar com Id preenchido</returns>
   [HttpPost]
   public AsyncResult Post([FromBody] CadastrarPessoaCommandV2 command) =>
      SendAsync(command, true);

   /// <summary>
   /// Alterar pessoa
   /// </summary>
   /// <returns>Pessoa cadastrar com Id preenchido</returns>
   [HttpPut("{id}")]
   public AsyncResult Put(Guid id, [FromBody] AlterarPessoaCommand command) =>
      SendAsync(command with { Id = id });

   /// <summary>
   /// Excluir uma pessoa cadastrada pelo seu id
   /// </summary>
   [HttpDelete("{id}")]
   public AsyncResult Delete(Guid id) =>
      SendAsync(new RemoverPessoaCommand(id));
}