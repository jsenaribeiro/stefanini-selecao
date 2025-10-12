namespace Api.Service.Contracts;

public record PagedList(int Total);

public record PagedList<E>(List<E> Items, int Total): PagedList(Total);