using AutoMapper.Configuration.Annotations;
using Stushbr.Shared.DataAccess;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Stushbr.PaymentsGatewayWeb.Models;

public record Item(
    string Id,
    string DisplayName,
    string Description,
    double Price,
    ItemType Type,
    JsonObject ItemData,
    bool IsEnabled,
    DateTime AvailableSince,
    DateTime AvailableBefore
) : IIdentifier
{
    [Ignore]
    [JsonIgnore]
    [IgnoreDataMember]
    public bool IsAvailable =>
        IsEnabled
        && DateTime.Now > AvailableSince
        && DateTime.Now < AvailableBefore;
}
