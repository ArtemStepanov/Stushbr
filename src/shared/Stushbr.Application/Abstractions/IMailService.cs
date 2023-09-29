using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Application.Abstractions;

public interface IMailService
{
    Task SendTelegramInviteLinkAsync(ClientItem clientItem);
}
