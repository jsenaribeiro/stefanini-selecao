using Api.Domain;

/// <summary>
/// Resultado de uma lista paginada
/// </summary>
/// <typeparam name="E">Entidade</typeparam>
/// <typeparam name="I">Identidade</typeparam>
/// <param name="sum">Total de registros da consulta</param>
/// <param name="size">Quantidade de linhas por pagina</param>
/// <param name="pages">Total de páginas (calculado)</param>
/// <param name="number">Número da página atual</param>
/// <param name="records">Lista de entidades retornadas</param>
public record PageListResult<E>(int sum, int size, int pages, int number, E[] records)
{
   public PageListResult(PageList<E> pageList, int size, int number)
      : this(pageList.Total, size, 0, number, pageList.Items)
   {
      this.pages = pageList.Total / number;
   }

   public PageListResult(PageList<E> pageList, Page page)
      : this(pageList, page.Length, page.Number) { }
}