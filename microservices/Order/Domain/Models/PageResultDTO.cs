namespace Domain.Models;

public class PagedResultDTO<T>
{
    /// <summary>
    /// Данные текущей страницы.
    /// </summary>
    public IEnumerable<T> Data { get; set; } = null!;

    /// <summary>
    /// Общее количество записей, удовлетворяющих условию.
    /// </summary>
    public int TotalRecords { get; set; }
}