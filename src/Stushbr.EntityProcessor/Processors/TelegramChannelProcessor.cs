using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor : ITelegramChannelProcessor
{
    private readonly ILogger<TelegramChannelProcessor> _logger;
    private readonly ITelegramService _telegramService;
    private readonly IMailService _mailService;
    private readonly StushbrDbContext _dbContext;

    public TelegramChannelProcessor(
        ILogger<TelegramChannelProcessor> logger,
        ITelegramService telegramService,
        IMailService mailService,
        StushbrDbContext dbContext
    )
    {
        _logger = logger;
        _telegramService = telegramService;
        _mailService = mailService;
        _dbContext = dbContext;
    }

    public async Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken)
    {
        LogInformation("Processing telegram item", clientItem);

        if (!clientItem.TelegramData.Any())
        {
            LogInformation("Invitation links are not exist and will be generated", clientItem);

            await GenerateInviteLinksAndUpdateClientItemAsync(
                clientItem,
                clientItem.Item!.TelegramItem!.Channels.Select(x => x.ChannelId),
                cancellationToken
            );
        }

        LogInformation("Sending link to mail", clientItem);
        await _mailService.SendTelegramInviteLinkAsync(clientItem);

        LogInformation("Updating client item state to processed", clientItem);

        clientItem.SetProcessed(true);

        _dbContext.ClientItems.Update(clientItem);
        await _dbContext.SaveChangesAsync(cancellationToken);

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
            var link = await _telegramService.CreateInviteLinkAsync(telegramChannelId, cancellationToken);
            var chatInfo = await _telegramService.GetChatInfoAsync(telegramChannelId, cancellationToken);
            clientItem.TelegramData.Add(new TelegramClientItem
            {
                InviteLink = link.InviteLink,
                LinkExpireDate = link.ExpireDate,
                ChannelName = chatInfo.Title ?? throw new ArgumentException("Chat title is null", nameof(chatInfo.Title))
            });
        }

        _dbContext.ClientItems.Update(clientItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void LogInformation(string text, ClientItem clientItem)
    {
        _logger.LogInformation("[CI '{ItemId}'] {Text}", clientItem.Id, text);
    }
}