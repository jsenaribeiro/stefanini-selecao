using Api.Domain.Pessoas;

namespace Api.Infrastructure.Repositories;

public class PessoaRepository : AbstractRepository<Pessoa>, IPessoaRepository
{
   public PessoaRepository(IServiceProvider provider) : base(provider) { }
}