using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Application.Abstractions;

public interface IClientService : ICrudService<Client>
{
    Task<Client?> TryGetClientByEmailAsync(string email);
}
