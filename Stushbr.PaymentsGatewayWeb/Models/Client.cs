using LinqToDB.Mapping;
using Stushbr.Shared.DataAccess;

namespace Stushbr.PaymentsGatewayWeb.Models;

[Table("Clients")]
public class Client : IIdentifier
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Column, NotNull]
    public string FirstName { get; init; }

    [Column, NotNull]
    public string SecondName { get; init; }

    [Column, NotNull]
    public string Email { get; init; }

    [Column, NotNull]
    public string PhoneNumber { get; init; }

    [LinqToDB.Mapping.Association(ThisKey = nameof(Id), OtherKey = nameof(Bill.ClientId), Relationship = Relationship.OneToMany)]
    public List<Bill> Bills { get; set; }
}
