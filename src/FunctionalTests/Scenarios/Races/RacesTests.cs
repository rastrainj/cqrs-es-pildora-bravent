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
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var location = "Pamplona";

        var request = new PlanRaceRequest(date, null, location, null, null);

        var response = await _client
            .PostAsJsonAsync("api/races", request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task plan_race_ok()
    {
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var hour = new TimeOnly(9, 0);
        var town = "Pamplona";
        var distance = 100;
        var elevationGain = 6_000;

        var request = new PlanRaceRequest(date, hour, town, distance, elevationGain);

        var response = await _client
            .PostAsJsonAsync("api/races", request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        var id = await response.Content.ReadAsAsync<Guid>();

        id.Should().NotBeEmpty();
    }
}
