using TrailRunning.Races.Core.Response;
using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.FunctionalTests.Extensions;
using TrailRunning.Races.Management.Host.Features.Races.GetAllRaces;
using TrailRunning.Races.Management.Host.Features.Races.GetRaceById;
using TrailRunning.Races.Management.Host.Features.Races.PlanRace;

namespace TrailRunning.Races.Management.FunctionalTests.Scenarios.Races;

[Collection(nameof(AspNetCoreServer))]
public class races_controller_should
{
    private readonly TestingWebAppFactory _testingWebAppFactory;
    private readonly HttpClient _client;

    public races_controller_should(TestingWebAppFactory testingWebAppFactory)
    {
        _testingWebAppFactory = testingWebAppFactory;
        _client = _testingWebAppFactory.CreateClient();
    }

    [Fact]
    public async Task plan_race_conflict_if_date_is_past()
    {
        var name = "UTMB";
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var location = "Pamplona";

        var request = new PlanRaceRequest(name, date, null, location, null, null);

        var response = await _client
            .PostAsJsonAsync("api/races", request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task plan_race_ok()
    {
        var name = "UTMB";
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var hour = new TimeOnly(9, 0);
        var town = "Pamplona";
        var distance = 100;
        var elevationGain = 6_000;

        var request = new PlanRaceRequest(name, date, hour, town, distance, elevationGain);

        var response = await _client
            .PostAsJsonAsync("api/races", request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        var id = await response.Content.ReadAsAsync<Guid>();

        id.Should().NotBeEmpty();

        var raceAggregate = await _testingWebAppFactory.Given
            .GetAsync<Race>(id);

        raceAggregate.Should().NotBeNull();
        raceAggregate!.Version.Should().Be(1);
        raceAggregate!.Status.Should().Be(RaceStatus.Planned);
        raceAggregate!.Name.Name.Should().BeEquivalentTo(name);
        raceAggregate!.Date.Date.Should().BeEquivalentTo(date);
        raceAggregate!.Date.Hour.Should().BeEquivalentTo(hour);
        raceAggregate!.Location.Town.Should().Be(town);
        raceAggregate!.TechnicalData!.Distance.Should().Be(distance);
        raceAggregate!.TechnicalData!.ElevationGain.Should().Be(elevationGain);
    }

    [Fact]
    [ResetDatabase]
    public async Task get_all_when_no_data()
    {
        var response = await _client.GetAsync("api/races");

        response.StatusCode
            .Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadAsAsync<PagedListResponse<RaceShortInfo>>();

        content.Should().NotBeNull();
        content!.TotalItemCount.Should().Be(0);
        content.Items.Should().HaveCount(0);
    }

    [Fact]
    [ResetDatabase]
    public async Task get_all_ok()
    {
        var name = "UTMB";
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var hour = new TimeOnly(9, 0);
        var town = "Pamplona";
        var distance = 100;
        var elevationGain = 6_000;

        var race = Race.Plan(Guid.NewGuid(), RaceName.Create(name), RaceDate.Create(date, hour), RaceLocation.Create(town), RaceTechnicalData.Create(distance, elevationGain));
        await _testingWebAppFactory.Given.AddAsync(race);

        var response = await _client.GetAsync("api/races");

        response.StatusCode
            .Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadAsAsync<PagedListResponse<RaceShortInfo>>();

        content.Should().NotBeNull();
        content!.TotalItemCount.Should().Be(1);
        content.Items.Should().HaveCount(1);

        var raceInfo = content.Items.First();
        raceInfo.Id.Should().Be(race.Id);
        raceInfo.Status.Should().Be(RaceStatus.Planned);
        raceInfo.Name.Should().Be(name);
    }

    [Fact]
    public async Task get_byid_notfound_if_not_exists()
    {
        var response = await _client.GetAsync($"api/races/{Guid.NewGuid()}");

        response.StatusCode
            .Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task get_byid_ok()
    {
        var name = "UTMB";
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var hour = new TimeOnly(9, 0);
        var town = "Pamplona";
        var distance = 100;
        var elevationGain = 6_000;

        var race = Race.Plan(Guid.NewGuid(), RaceName.Create(name), RaceDate.Create(date, hour), RaceLocation.Create(town), RaceTechnicalData.Create(distance, elevationGain));
        await _testingWebAppFactory.Given.AddAsync(race);

        var response = await _client.GetAsync($"api/races/{race.Id}");

        response.StatusCode
            .Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadAsAsync<RaceDetails>();

        content.Should().NotBeNull();

        content!.Id.Should().Be(race.Id);
        content.Status.Should().Be(race.Status);
        content.Name.Should().Be(name);
        content.Date.Should().Be(date);
        content.Hour.Should().Be(hour);
        content.Town.Should().Be(town);
        content.Distance.Should().Be(distance);
        content.ElevationGain.Should().Be(elevationGain);
    }
}
