using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.Domain.Races;

public class RaceLocation
{
    private RaceLocation(string town) => Town = town;

    public string Town { get; private set; } = default!;

    public static RaceLocation Create(string town)
        => string.IsNullOrWhiteSpace(town)
            ? throw new RaceLocationException("El nombre del lugar de la carrera debe ser especificado.")
            : (new(town));
}
