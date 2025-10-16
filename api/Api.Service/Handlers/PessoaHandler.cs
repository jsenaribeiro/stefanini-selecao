using MediatR;
using Api.Domain;
using Api.Domain.Pessoas;
using Api.Infrastructure.Values;
using Api.Service.Queries;
using Api.Service.Commands;
using Api.Service.Results;
using Microsoft.EntityFrameworkCore;

namespace Api.Service.Handlers;

public class PessoaHandler : AbstractHandler
   , IRequestHandler<ConsultarPessoasQuery, PageList<PessoaResult>>
   , IRequestHandler<CadastrarPessoaCommand, PessoaResult>
   , IRequestHandler<AlterarPessoaCommand, PessoaResult>
   , IRequestHandler<RemoverPessoaCommand, bool>
{
   public PessoaHandler(IServiceProvider provider) : base(provider) { }

   public async Task<PageList<PessoaResult>> Handle(ConsultarPessoasQuery query, CancellationToken cancel)
   {
      if (query is null) throw new ArgumentNullException(nameof(ConsultarPessoasQuery));

      var unitOfWork = provider.GetRequiredService<IUnitOfWork>();

      var nome = string.IsNullOrWhiteSpace(query.Nome) ? null : query.Nome.ToLower();

      var (items, total) = await unitOfWork.Pessoas      
         .PageBy(query.Page.Size, query.Page.Number)
         .OrderBy(query.Sort.Field, query.Sort.Order)
         .FilterBy(p => nome == null || p.Nome.ToLower().Contains(nome))
         .ListAsync();

      return PessoaResult.From(total, items);
   }

   public async Task<PessoaResult> Handle(CadastrarPessoaCommand command, CancellationToken cancel)
   {
      ArgumentNullException.ThrowIfNull(command, nameof(ConsultarPessoasQuery));

      var pessoa = new Pessoa(command.Nome, command.Nascimento)
      {
         CPF = command.CPF,
         Sexo = command.Sexo,
         Email = command.Email,
         Nacionalidade = command.Nacionalidade
      };

      Invalid.ThrowIfInvalid(command, provider);
      Invalid.ThrowIfInvalid(pessoa, provider);

      pessoa = await unitOfWork.Pessoas.CreateAsync(pessoa);

      return new PessoaResult(pessoa);
   }

   public async Task<PessoaResult> Handle(AlterarPessoaCommand command, CancellationToken cancel)
   {
      ArgumentNullException.ThrowIfNull(command, nameof(AlterarPessoaCommand));

      var pessoa = await unitOfWork.Pessoas.LoadAsync(command.Id);

      if (pessoa is null) throw Errors.NotFound(nameof(Pessoa));

      pessoa.Nome = command.Nome ?? pessoa.Nome;
      pessoa.CPF = command.CPF ?? pessoa.CPF;
      pessoa.Sexo = command.Sexo ?? pessoa.Sexo;
      pessoa.Email = command.Email ?? pessoa.Email;
      pessoa.Nascimento = command.Nascimento ?? pessoa.Nascimento;
      pessoa.Nacionalidade = command.Nacionalidade ?? pessoa.Nacionalidade;

      Invalid.ThrowIfInvalid(command, provider);
      Invalid.ThrowIfInvalid(pessoa, provider);

      await unitOfWork.Pessoas.UpdateAsync(pessoa);      

      return new PessoaResult(pessoa);
   }

   public async Task<bool> Handle(RemoverPessoaCommand command, CancellationToken cancel)
   {
      ArgumentNullException.ThrowIfNull(command, nameof(RemoverPessoaCommand));

      var encontrou = await unitOfWork.Pessoas.ExistsAsync(command.Id);

      if (!encontrou) throw Errors.NotFound(nameof(Pessoa));

      await unitOfWork.Pessoas.DeleteAsync(command.Id);

      Invalid.ThrowIfInvalid(command, provider);

      return true;
   }
}