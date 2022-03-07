using TrailRunning.Races.Management.Domain.Races;
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
}
