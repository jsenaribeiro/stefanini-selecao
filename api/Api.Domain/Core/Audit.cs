namespace Api.Domain;

public abstract class Audit
{
   public DateTime DataCriacao { get; set; } = DateTime.Now;

   public DateTime? DataAtualizacao { get; set; }
}