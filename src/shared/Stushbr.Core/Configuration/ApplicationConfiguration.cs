using System.Text.Json.Serialization;

namespace Stushbr.Core.Configuration;

public class ApplicationConfiguration
{
    public QiwiConfiguration? Qiwi { get; set; }

    public string SuccessUrl { get; set; } = "https://stushbr.ru";
}