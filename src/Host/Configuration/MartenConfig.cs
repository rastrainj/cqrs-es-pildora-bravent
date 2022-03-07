using Marten.Events.Daemon.Resiliency;

namespace TrailRunning.Races.Management.Host.Configuration;

public class MartenConfig
{
#pragma warning disable IDE1006 // Naming Styles
    private const string DefaultSchema = "public";
#pragma warning restore IDE1006 // Naming Styles

    public string ConnectionString { get; set; } = default!;

    public string WriteModelSchema { get; set; } = DefaultSchema;
    public string ReadModelSchema { get; set; } = DefaultSchema;

    public bool ShouldRecreateDatabase { get; set; }

    public DaemonMode DaemonMode { get; set; } = DaemonMode.Disabled;
}
