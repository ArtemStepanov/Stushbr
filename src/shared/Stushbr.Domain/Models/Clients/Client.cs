using Stushbr.Domain.Abstractions;

namespace Stushbr.Domain.Models.Clients;

public class Client : IIdentifier
{
    public int Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string SecondName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public virtual ICollection<ClientItem> ClientItems { get; set; } = new List<ClientItem>();

    public string FullName => $"{FirstName} {SecondName}";
}