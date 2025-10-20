using Api.Domain.Pessoas;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Commands;

public record AlterarPessoaCommand : IRequest<PessoaResult>
{
   public AlterarPessoaCommand(Guid id) => Id = id;

   public Guid Id { get; set; }

   public Sexo? Sexo { get; set; }

   public string? CPF { get; set; }

   public string? Nome { get; set; }

   public string? Email { get; set; }

   public DateOnly? Nascimento { get; set; }

   public string? Nacionalidade { get; set; }
}
