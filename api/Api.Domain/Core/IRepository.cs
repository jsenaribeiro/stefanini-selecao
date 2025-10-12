namespace Api.Domain;

public interface IRepository<E> : IReadRepository<E>, IWriteRepository<E> where E : Entity
{

}