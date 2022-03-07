namespace TrailRunning.Races.Core.Response;

public class PagedListResponse<T>
{
    public IEnumerable<T> Items { get; }

    public long TotalItemCount { get; }

    public bool HasNextPage { get; }

    public PagedListResponse(IEnumerable<T> items, long totalItemCount, bool hasNextPage)
    {
        Items = items.ToList();
        TotalItemCount = totalItemCount;
        HasNextPage = hasNextPage;
    }
}
