using Api.Domain.Pessoas;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Commands;

public record CadastrarPessoaCommand
(
   char? Sexo,
   string Nome,
   string? Email,
   string Nascimento,
   string? Nacionalidade,
   string? CPF
)
: IRequest<PessoaResult>
{
   public CadastrarPessoaCommand(string nome, string nascimento)
      : this(null, nome, null, nascimento, null, null) { }

   public CadastrarPessoaCommand() : this(null, "",null, "", null, null) {}
}

