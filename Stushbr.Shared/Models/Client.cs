using LinqToDB.Mapping;
using Stushbr.Shared.DataAccess;

namespace Stushbr.Shared.Models;

[Table("clients")]
public class Client : IIdentifier
{
    [Column("id"), PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Column("first_name")]
    public string FirstName { get; init; }

    [Column("second_name")]
    public string SecondName { get; init; }

    [Column("email")]
    public string Email { get; init; }

    [Column("phone_number")]
    public string PhoneNumber { get; init; }

    [Association(ThisKey = nameof(Id), OtherKey = nameof(Bill.ClientId))]
    public List<Bill> Bills { get; set; }
}
