using Api.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Configurations;

public class PessoaConfiguration : AbstractConfiguration<Pessoa, Guid>
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

      etb.HasIndex(x => x.CPF)
         .IsUnique();

      etb.OwnsOne(x => x.Endereco, x =>
      {
         x.Property(p => p.Numero);
         x.Property(p => p.Bairro);
         x.Property(p => p.Pais);
         x.Property(p => p.Rua);
      });
   }
}