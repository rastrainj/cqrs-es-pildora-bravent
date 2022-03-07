using Marten;
using Marten.Pagination;
using TrailRunning.Races.Core.Response;

namespace TrailRunning.Races.Management.Host.Features.Races.GetAllRaces;

public static class GetAllRaces
{
    [ApiExplorerSettings(GroupName = "Races")]
    [ProducesResponseType(typeof(PagedListResponse<RaceShortInfo>), StatusCodes.Status200OK)]
    public static async Task<PagedListResponse<RaceShortInfo>> Handle(IMediator mediator)
    {
        var pagedList = await mediator.Send(new GetAllRacesQuery());
        return pagedList.ToResponse();
    }
}

public record GetAllRacesQuery : IRequest<IPagedList<RaceShortInfo>>;

public class GetAllRacesRequestHandler : IRequestHandler<GetAllRacesQuery, IPagedList<RaceShortInfo>>
{
    private readonly IDocumentSession _querySession;

    public GetAllRacesRequestHandler(IDocumentSession querySession)
        => _querySession = querySession ?? throw new ArgumentNullException(nameof(querySession));

    public Task<IPagedList<RaceShortInfo>> Handle(GetAllRacesQuery request, CancellationToken cancellationToken)
        => _querySession.Query<RaceShortInfo>().ToPagedListAsync(1, 20, cancellationToken);
}
