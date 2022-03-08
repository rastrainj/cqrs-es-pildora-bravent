using Marten.Pagination;

namespace TrailRunning.Races.Core.Response;

public static class PagedListExtensions
{
    public static PagedListResponse<T> ToResponse<T>(this IPagedList<T> pagedList) =>
        new(pagedList, pagedList.TotalItemCount, pagedList.HasNextPage);
}
