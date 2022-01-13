using LinqToDB;
using LinqToDB.Mapping;
using Stushbr.Shared.DataAccess;

namespace Stushbr.PaymentsGatewayWeb.Models;

[Table("Items")]
public class Item : IIdentifier
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [NotNull, Column]
    public string DisplayName { get; set; }

    [NotNull, Column]
    public string Description { get; set; }

    [NotNull, Column]
    public double Price { get; set; }

    [NotNull, Column]
    public ItemType Type { get; set; }

    [DataType(DataType.Json), Column]
    public dynamic? Data { get; set; }

    [NotNull, Column]
    public bool IsEnabled { get; set; } = true;

    [Column]
    public DateTime? AvailableSince { get; set; }

    [Column]
    public DateTime? AvailableBefore { get; set; }

    [Association(ThisKey = nameof(Id), OtherKey = nameof(Bill.ItemId), Relationship = Relationship.OneToMany)]
    public List<Bill> Bills { get; set; }
}
