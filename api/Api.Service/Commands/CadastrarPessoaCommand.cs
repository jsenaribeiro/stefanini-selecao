using Api.Domain.Pessoas;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Commands;

public record CadastrarPessoaCommand
(
   string Nome,
   char? Sexo,
   string? Email,
   string Nascimento,
   string? Nacionalidade,
   string? CPF
)
: IRequest<PessoaResult>
{
   public CadastrarPessoaCommand(string nome, string nascimento)
      : this(nome, null, null, nascimento, null, null) { }

   public CadastrarPessoaCommand() : this("", null, null, "", null, null) {}
}

