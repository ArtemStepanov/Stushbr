using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stushbr.AdminUtilsWeb.ViewModels.Items;
using Stushbr.Core.Enums;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;

namespace Stushbr.AdminUtilsWeb.Controllers;

public class ItemsController(StushbrDbContext dbContext) : Controller
{
    // todo: use mediatr

    // GET
    public async Task<IActionResult> Index()
    {
        var items = await dbContext.Items.Select(ItemViewModel.Create).ToListAsync();
        return View(items);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddItem(CreateItemViewModel model)
    {
        // todo: convert to mediatr command
        var item = new Item
        {
            DisplayName = model.DisplayName,
            Description = model.Description,
            Price = model.Price,
            ItemIdentifier = model.ItemIdentifier,
            Type = Enum.Parse<ItemType>(model.Type),
            IsEnabled = true,
            AvailableSince = model.AvailableSince,
            AvailableBefore = model.AvailableBefore
        };

        await dbContext.Items.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateItem(ItemViewModel model)
    {
        var item = await dbContext.Items.FindAsync(model.Id);
        if (item is null)
        {
            return NotFound();
        }

        item.DisplayName = model.DisplayName;
        item.Description = model.Description;
        item.Price = model.Price;
        item.ItemIdentifier = model.ItemIdentifier;
        item.Type = Enum.Parse<ItemType>(model.Type);
        item.IsEnabled = model.IsEnabled;
        item.AvailableSince = model.AvailableSince;
        item.AvailableBefore = model.AvailableBefore;

        await dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await dbContext.Items.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        dbContext.Remove(item);
        await dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpsertTelegramItem(int itemId, TelegramItemViewModel model)
    {
        var telegramItem = await dbContext.TelegramItems.Include(x => x.Channels).FirstOrDefaultAsync(x => x.Id == model.Id);
        if (telegramItem is null)
        {
            telegramItem = new TelegramItem
            {
                SendPulseTemplateId = model.SendPulseTemplateId,
                Channels = model.ChannelIds.Split(',').Select(x => new TelegramItemChannel
                {
                    ChannelId = long.Parse(x)
                }).ToList(),
                ItemId = itemId
            };

            await dbContext.TelegramItems.AddAsync(telegramItem);
        }
        else
        {
            telegramItem.SendPulseTemplateId = model.SendPulseTemplateId;

            dbContext.RemoveRange(telegramItem.Channels);
            telegramItem.Channels = model.ChannelIds.Split(',').Select(x => new TelegramItemChannel
            {
                ChannelId = long.Parse(x)
            }).ToList();
        }

        await dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}