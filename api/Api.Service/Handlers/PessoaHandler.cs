using MediatR;
using Api.Domain;
using Api.Domain.Pessoas;
using Api.Infrastructure.Values;
using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Server;
using Api.Service.Queries;
using Api.Service.Commands;
using NLog.LayoutRenderers;
using Api.Service.Results;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Service.Handlers;

public class PessoaHandler : AbstractHandler
   , IRequestHandler<ConsultarPessoasQuery, PageList<PessoaResult>>
   , IRequestHandler<CadastrarPessoaCommand, PessoaResult>
{
   public PessoaHandler(IServiceProvider provider) : base(provider) { }

   public async Task<PageList<PessoaResult>> Handle(ConsultarPessoasQuery query, CancellationToken cancel)
   {
      if (query is null) throw new ArgumentNullException(nameof(ConsultarPessoasQuery));

      var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
      var nome = string.IsNullOrWhiteSpace(query.Nome) ? null : query.Nome.ToLower();
      var exp = from p in unitOfWork.Pessoas.Query
                where nome == null || p.Nome.ToLower().Contains(nome)
                select p;

      var (items, total) = await exp.ToPageListAsync(query.Page, query.Sort);


      return PessoaResult.From(total, items.ToArray());
   }

   public async Task<PessoaResult> Handle(CadastrarPessoaCommand command, CancellationToken cancel)
   {
      ArgumentNullException.ThrowIfNull(command, nameof(ConsultarPessoasQuery));

      var isValidSexo = Enum.TryParse<Sexo>(command.Sexo.ToString(), out var sexo);

      var pessoa = new Pessoa(command.Nome, command.Nascimento)
      {
         CPF = command.CPF,
         Email = command.Email,
         Nacionalidade = command.Nacionalidade,
         Sexo = isValidSexo ? sexo : null
      };

      Invalid.ThrowIfInvalid(pessoa, provider);

      pessoa = await unitOfWork.Pessoas.SaveAsync(pessoa);

      return new PessoaResult(pessoa);
   }
}