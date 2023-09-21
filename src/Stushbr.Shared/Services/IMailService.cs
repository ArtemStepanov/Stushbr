using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public interface IMailService
{
    Task SendTelegramInviteLinkAsync(ClientItem clientItem);
}
