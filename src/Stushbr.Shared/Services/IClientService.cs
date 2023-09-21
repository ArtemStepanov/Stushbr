using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public interface IClientService : ICrudService<Client>
{
    Task<Client?> TryGetClientByEmailAsync(string email);
}
