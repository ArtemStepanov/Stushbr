using Stushbr.Domain.Abstractions;

namespace Stushbr.Domain.Models;

public class Client : IIdentifier
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string FirstName { get; set; } = default!;

    public string SecondName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public virtual ICollection<ClientItem> ClientItems { get; set; } = new List<ClientItem>();

    public string FullName => $"{FirstName} {SecondName}";
}