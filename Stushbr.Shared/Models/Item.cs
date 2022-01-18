using System.Text.Json.Nodes;
using LinqToDB;
using LinqToDB.Mapping;
using Stushbr.Shared.DataAccess;

namespace Stushbr.Shared.Models;

[Table("items")]
public class Item : IIdentifier
{
    [Column("id"), PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Column("display_name")]
    public string DisplayName { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("price")]
    public double Price { get; set; }

    [Column("type")]
    public ItemType Type { get; set; }

    [Column("data", DataType = DataType.Json)]
    public JsonNode Data { get; set; }

    [Column("is_enabled")]
    public bool IsEnabled { get; set; } = true;

    [Column("available_since")]
    public DateTime AvailableSince { get; set; } = DateTime.Now;

    [Column("available_before")]
    public DateTime? AvailableBefore { get; set; }

    [Association(ThisKey = nameof(Id), OtherKey = nameof(ClientItem.ItemId))]
    public List<ClientItem> ClientItems { get; set; }
}
