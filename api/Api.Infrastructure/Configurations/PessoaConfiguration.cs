using Api.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Configurations;

public class PessoaConfiguration : AbstractConfiguration<Pessoa>
{
   public override void Configure(EntityTypeBuilder<Pessoa> etb)
   {
      base.Configure(etb);

      etb.Property(x => x.Nome);
      etb.Property(x => x.Nascimento);
      etb.Property(x => x.Nacionalidade);
      etb.Property(x => x.Email);
      etb.Property(x => x.CPF);

      etb.Property(u => u.Sexo)
         .HasConversion<string>() 
         .IsRequired(false);
   }
}