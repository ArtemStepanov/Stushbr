using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor(
    ILogger<TelegramChannelProcessor> logger,
    ITelegramService telegramService,
    IMailService mailService,
    StushbrDbContext dbContext
) : ITelegramChannelProcessor
{
    public async Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken)
    {
        LogInformation("Processing telegram item", clientItem);

        if (clientItem.TelegramData.Count == 0)
        {
            LogInformation("Invitation links are not exist and will be generated", clientItem);

            await GenerateInviteLinksAndUpdateClientItemAsync(
                clientItem,
                clientItem.Item!.TelegramItem!.Channels.Select(x => x.ChannelId),
                cancellationToken
            );
        }

        LogInformation("Sending link to mail", clientItem);
        await mailService.SendTelegramInviteLinkAsync(clientItem);

        LogInformation("Updating client item state to processed", clientItem);

        clientItem.SetProcessed(true);

        dbContext.ClientItems.Update(clientItem);
        await dbContext.SaveChangesAsync(cancellationToken);

        LogInformation("Item processed", clientItem);
    }

    private async Task GenerateInviteLinksAndUpdateClientItemAsync(
        ClientItem clientItem,
        IEnumerable<long> telegramChannelIds,
        CancellationToken cancellationToken
    )
    {
        foreach (var telegramChannelId in telegramChannelIds)
        {
            var link = await telegramService.CreateInviteLinkAsync(telegramChannelId, cancellationToken);
            var chatInfo = await telegramService.GetChatInfoAsync(telegramChannelId, cancellationToken);
            clientItem.TelegramData.Add(new TelegramClientItem
            {
                InviteLink = link.InviteLink,
                LinkExpireDate = link.ExpireDate,
                ChannelName = chatInfo.Title ?? throw new ArgumentException("Chat title is null", nameof(chatInfo.Title))
            });
        }

        dbContext.ClientItems.Update(clientItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private void LogInformation(string text, ClientItem clientItem)
    {
        logger.LogInformation("[CI '{ItemId}'] {Text}", clientItem.Id, text);
    }
}