using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.Domain.Races;

public class RaceName
{
    private RaceName(string name) => Name = name;

    public string Name { get; private set; } = default!;

    public static RaceName Create(string name)
        => string.IsNullOrWhiteSpace(name)
            ? throw new RaceNameException("El nombre de la carrera debe ser especificado.")
            : (new(name));
}
