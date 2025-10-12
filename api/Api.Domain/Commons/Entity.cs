namespace Api.Domain;

public abstract class Entity : Audit
{
   public Guid Id { get; set; } = Guid.Empty;
}