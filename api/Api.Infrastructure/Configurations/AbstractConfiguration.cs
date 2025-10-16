using Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Configurations;

public abstract class AbstractConfiguration<E,I> 
   : IEntityTypeConfiguration<E> 
     where E : Entity<I>
     where I : struct
{
   private readonly string tableName;

   public AbstractConfiguration() => tableName = typeof(E).Name + "s";

   public AbstractConfiguration(string tableName) => this.tableName = tableName;

   public virtual void Configure(EntityTypeBuilder<E> builder)
   {
      builder.ToTable(tableName);
      
      builder.HasKey(u => u.Id);

      builder.OwnsOne(x => x.Log, log =>
      {
         log.Property(x => x.DataCriacao);
         log.Property(x => x.DataAtualizacao);
      });
   }
}