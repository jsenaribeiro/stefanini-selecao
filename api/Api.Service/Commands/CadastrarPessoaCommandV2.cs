using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Api.Domain.Pessoas;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Commands;

public record CadastrarPessoaCommandV2 : IRequest<PessoaResult>
{
   public CadastrarPessoaCommandV2()
   {
      Nome = "";
      Nascimento = default;
   }

   public CadastrarPessoaCommandV2(string nome, DateOnly nascimento)
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

   public Endereco? Endereco { get; set; }
}