using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stushbr.AdminUtilsWeb.ViewModels;
using Stushbr.AdminUtilsWeb.ViewModels.Items;
using Stushbr.Core.Enums;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;

namespace Stushbr.AdminUtilsWeb.Controllers;

public class ItemsController : Controller
{
    private readonly StushbrDbContext _dbContext;

    public ItemsController(StushbrDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var items = await _dbContext.Items.Select(ItemViewModel.Create).ToListAsync();
        return View(items);
    }

    [HttpPost]
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

        await _dbContext.Items.AddAsync(item);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}")]
    public async Task<IActionResult> UpdateItem(int id, ItemViewModel model)
    {
        var item = await _dbContext.Items.FindAsync(id);
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

        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _dbContext.Items.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        _dbContext.Remove(item);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("telegram")]
    public async Task<IActionResult> UpsertTelegramItem(TelegramItemViewModel model)
    {
        var telegramItem = await _dbContext.TelegramItems.FindAsync(model.Id);
        if (telegramItem is null)
        {
            telegramItem = new TelegramItem
            {
                SendPulseTemplateId = model.SendPulseTemplateId,
                Channels = model.ChannelIds.Split(',').Select(x => new TelegramItemChannel
                {
                    ChannelId = long.Parse(x)
                }).ToList()
            };

            await _dbContext.TelegramItems.AddAsync(telegramItem);
        }
        else
        {
            telegramItem.SendPulseTemplateId = model.SendPulseTemplateId;
            telegramItem.Channels = model.ChannelIds.Split(',').Select(x => new TelegramItemChannel
            {
                ChannelId = long.Parse(x)
            }).ToList();
        }

        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}