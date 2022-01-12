using System.Text.Json.Serialization;

namespace Stushbr.PaymentsGatewayWeb.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemType
{
    TelegramChannel,
    YouTubeVideo,
    Other
}
