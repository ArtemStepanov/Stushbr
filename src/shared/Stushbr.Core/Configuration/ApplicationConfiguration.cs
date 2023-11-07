namespace Stushbr.Core.Configuration;

public class ApplicationConfiguration
{
    public QiwiConfiguration? Qiwi { get; set; }

    public string SuccessUrl { get; set; } = "https://stushbr.ru";

    public bool MigrationMode { get; set; }
}