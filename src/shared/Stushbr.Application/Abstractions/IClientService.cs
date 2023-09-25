using Stushbr.Domain.Models;

namespace Stushbr.Application.Abstractions;

public interface IClientService : ICrudService<Client>
{
    Task<Client?> TryGetClientByEmailAsync(string email);
}
