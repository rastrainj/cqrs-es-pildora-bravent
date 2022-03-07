using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.Domain.Races.Events;
using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.DomainTests.Races;

public class races_should
{
    [Fact]
    public void not_allow_create_date_if_past()
    {
        var date = DateTime.Now.AddDays(-1);
        var hour = new TimeOnly(9, 0);

        var act = () => RaceDate.Create(DateOnly.FromDateTime(date), hour);

        act.Should().Throw<RaceDatePastException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void not_allow_create_location_if_empty(string location)
    {
        var act = () => RaceLocation.Create(location);

        act.Should().Throw<RaceLocationException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    public void not_allow_create_technicaldata_if_distance_negative_or_0(double distance)
    {
        var act = () => RaceTechnicalData.Create(distance, 1_000);

        act.Should().Throw<RaceTechnicalDataException>();
    }

    [Fact]
    public void not_allow_create_technicaldata_if_elevationgain_negative()
    {
        var act = () => RaceTechnicalData.Create(100, -0.1);

        act.Should().Throw<RaceTechnicalDataException>();
    }

    [Fact]
    public void allow_plan_and_status_planned()
    {
        var raceId = Guid.NewGuid();
        var date = RaceDate.Create(DateOnly.FromDateTime(DateTime.Now.AddDays(10)), new(9, 0));
        var location = RaceLocation.Create("Pamplona");
        var technicalData = RaceTechnicalData.Create(100, 6_000);

        var race = Race.Plan(raceId, date, location, technicalData);

        race.Should().NotBeNull();
        race.Version.Should().Be(1);
        race.Status.Should().Be(RaceStatus.Planned);
        race.Id.Should().Be(raceId);
        race.Date.Should().BeEquivalentTo(date);
        race.Location.Should().BeEquivalentTo(location);
        race.TechnicalData.Should().BeEquivalentTo(technicalData);

        var @event = race.PublishedEvent<RacePlannedEvent>();
        @event.Should().NotBeNull();
        @event.Should().BeOfType<RacePlannedEvent>();
        @event!.RaceId.Should().Be(race.Id);
    }
}
