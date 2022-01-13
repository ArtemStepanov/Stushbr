using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

public interface IClientService : ICrudService<Client>
{
    Task<Client?> TryGetClientByEmailAsync(string email);
}
