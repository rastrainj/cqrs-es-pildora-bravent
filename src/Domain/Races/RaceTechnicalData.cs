using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.Domain.Races;

public class RaceTechnicalData
{
    private RaceTechnicalData(double distance, double elevationGain)
    {
        Distance = distance;
        ElevationGain = elevationGain;
    }


    public double Distance { get; private set; }
    public double ElevationGain { get; private set; }

    public static RaceTechnicalData Create(double distance, double elevationGain) =>
        distance <= 0
            ? throw new RaceTechnicalDataException("La distancia de carrera debe ser un número positivo")
            : elevationGain < 0
            ? throw new RaceTechnicalDataException("El desnivel positivo de carrera debe ser un número positivo o 0")
            : new(distance, elevationGain);
}
