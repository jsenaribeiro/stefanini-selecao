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

      etb.OwnsOne(x => x.Email, email => email.Property(e => e.email));

      etb.OwnsOne(x => x.CPF, cpf =>
      {
         cpf.Property(c => c.Numero).HasColumnName("cpf");
         cpf.Ignore(c => c.IsValid);
      });

      etb.Property(u => u.Sexo)
         .HasConversion<string>() 
         .IsRequired(false);
   }
}