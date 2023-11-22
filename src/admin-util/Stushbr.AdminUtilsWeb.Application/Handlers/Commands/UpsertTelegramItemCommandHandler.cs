using Microsoft.EntityFrameworkCore;
using Stushbr.AdminUtilsWeb.Domain.Items.Commands;
using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;

namespace Stushbr.AdminUtilsWeb.Application.Handlers.Commands;

public sealed class UpsertTelegramItemCommandHandler(StushbrDbContext dbContext) : BaseRequestHandler<UpsertTelegramItemCommand, TelegramItemViewModel>(dbContext)
{
    public override async Task<TelegramItemViewModel> Handle(UpsertTelegramItemCommand request, CancellationToken cancellationToken)
    {
        var telegramItem = await dbContext.TelegramItems.Include(x => x.Channels).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (telegramItem is null)
        {
            telegramItem = new TelegramItem
            {
                SendPulseTemplateId = request.SendPulseTemplateId,
                Channels = request.ChannelIds.Split(',').Select(x => new TelegramItemChannel
                {
                    ChannelId = long.Parse(x)
                }).ToList(),
                ItemId = request.ItemId
            };

            await dbContext.TelegramItems.AddAsync(telegramItem, cancellationToken);
        }
        else
        {
            telegramItem.SendPulseTemplateId = request.SendPulseTemplateId;

            dbContext.RemoveRange(telegramItem.Channels);
            telegramItem.Channels = request.ChannelIds.Split(',').Select(x => new TelegramItemChannel
            {
                ChannelId = long.Parse(x)
            }).ToList();
        }

        return TelegramItemViewModel.Create(telegramItem);
    }
}