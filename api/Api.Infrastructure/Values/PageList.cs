namespace Api.Infrastructure.Values;

public record PageList(int Total);

public record PageList<E>(E[] Items, int Total): PageList(Total);