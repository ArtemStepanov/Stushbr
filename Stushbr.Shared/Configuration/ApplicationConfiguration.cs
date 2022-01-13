namespace Stushbr.Shared.Configuration;

public class ApplicationConfiguration
{
    public QiwiConfiguration? Qiwi { get; set; }

    public PostgresConfiguration? Postgres { get; set; }
    
    public string SuccessUrl { get; set; } = "https://stushbr.ru";
}
