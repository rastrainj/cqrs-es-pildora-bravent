namespace TrailRunning.Races.Management.Domain.Races.Exceptions;

public class RaceDatePastException : Exception
{
    public RaceDatePastException(string message)
        : base(message)
    {

    }
}
