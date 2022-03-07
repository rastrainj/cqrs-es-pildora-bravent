using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.DomainTests.Races;

public class races_should
{
    [Fact]
    public void not_allow_create_if_date_is_past()
    {
        var date = DateTime.Now.AddDays(-1);
        var hour = new TimeOnly(9, 0);

        var act = () => RaceDate.Create(DateOnly.FromDateTime(date), hour);

        act.Should().Throw<RaceDatePastException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void not_allow_create_if_location_is_empty(string location)
    {
        var act = () => RaceLocation.Create(location);

        act.Should().Throw<RaceLocationException>();
    }
}
