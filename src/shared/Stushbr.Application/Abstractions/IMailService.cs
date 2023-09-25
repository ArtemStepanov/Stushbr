using Stushbr.Domain.Models;

namespace Stushbr.Application.Abstractions;

public interface IMailService
{
    Task SendTelegramInviteLinkAsync(ClientItem clientItem);
}
