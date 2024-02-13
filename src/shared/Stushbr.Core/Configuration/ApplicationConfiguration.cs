namespace Stushbr.Core.Configuration;

public sealed class ApplicationConfiguration
{
    public QiwiConfiguration? Qiwi { get; set; }

    public string SuccessUrl { get; set; } = "https://stushbr.com";

    public bool MigrationMode { get; set; }
}