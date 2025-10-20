using Api.Domain.Pessoas;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Commands;

public record CadastrarPessoaCommand : IRequest<PessoaResult>
{
   public CadastrarPessoaCommand()
   {
      Nome = "";
      Nascimento = default;
   }

   public CadastrarPessoaCommand(string nome, DateOnly nascimento)
   {
      Nome = nome;
      Nascimento = nascimento;
   }

   public Sexo? Sexo { get; set; }

   public string? CPF { get; set; }

   public string Nome { get; set; }

   public string? Email { get; set; }

   public DateOnly Nascimento { get; set; }

   public string? Nacionalidade { get; set; }
}