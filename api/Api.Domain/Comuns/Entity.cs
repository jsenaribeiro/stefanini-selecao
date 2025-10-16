namespace Api.Domain;

public abstract class Entity<I> where I : struct
{
   public I Id { get; set; } = default;

   public Audit Log { get; set; } = new();
}

public record Audit
{
   public DateTime DataCriacao = default;

   public DateTime? DataAtualizacao;

   public static Audit Criacao => new Audit { DataCriacao = DateTime.Now };

   public static Audit Atualizacao => new Audit { DataAtualizacao = DateTime.Now };
}