using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.Domain.Races;

public class RaceDate
{
    private RaceDate(DateOnly date, TimeOnly? hour)
    {
        Date = date;
        Hour = hour;
    }

    public DateOnly Date { get; private set; } = default!;
    public TimeOnly? Hour { get; private set; }

    public static RaceDate Create(DateOnly date, TimeOnly? hour)
        => date < DateOnly.FromDateTime(DateTime.Today)
            ? throw new RaceDatePastException("La fecha de la carrera es del pasado")
            : (new(date, hour));
}
