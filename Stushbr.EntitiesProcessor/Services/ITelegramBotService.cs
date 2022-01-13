namespace Stushbr.EntitiesProcessor.Services;

public interface ITelegramBotService
{
    Task<string> CreateInviteLinkAsync(string channel);
}
