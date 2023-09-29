using Stushbr.Core.Enums;
using System.Text.Json.Serialization;

namespace Stushbr.Core.Configuration;

public class ApplicationConfiguration
{
    public QiwiConfiguration? Qiwi { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DatabaseType DatabaseType { get; set; } = DatabaseType.SqlServer;

    public string SuccessUrl { get; set; } = "https://stushbr.ru";
}