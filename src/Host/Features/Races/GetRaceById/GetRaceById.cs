using Marten;

namespace TrailRunning.Races.Management.Host.Features.Races.GetRaceById;

public static class GetRaceById
{
    [ApiExplorerSettings(GroupName = "Races")]
    [ProducesResponseType(typeof(RaceDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> Handle(Guid raceId, IMediator mediator)
    {
        var race = await mediator.Send(new GetRaceQuery(raceId));
        return race is not null ? Results.Ok(race) : Results.NotFound();
    }

    public record GetRaceQuery(Guid RaceId) : IRequest<RaceDetails?>;

    public class GetRaceQueryHandler : IRequestHandler<GetRaceQuery, RaceDetails?>
    {
        private readonly IQuerySession _querySession;

        public GetRaceQueryHandler(IQuerySession querySession)
            => _querySession = querySession ?? throw new ArgumentNullException(nameof(querySession));

        public Task<RaceDetails?> Handle(GetRaceQuery request, CancellationToken cancellationToken)
            => _querySession.LoadAsync<RaceDetails>(request.RaceId, cancellationToken);
    }
}
