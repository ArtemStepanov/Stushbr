using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stushbr.AdminUtilsWeb.Attributes;
using Stushbr.AdminUtilsWeb.Domain.Items.Commands;
using Stushbr.AdminUtilsWeb.Domain.Items.Queries;
using Stushbr.Api.Abstractions;

namespace Stushbr.AdminUtilsWeb.Controllers;

[AuthorizeAdmin]
public class ItemsController(ISender sender) : StushbrControllerBase(sender)
{
    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
    public async Task<IActionResult> Index() =>
        await CallHandlerAsync(new GetItemsQuery(), View);

    [HttpPost]
    public async Task<IActionResult> AddItem(CreateItemCommand command) =>
        await CallHandlerAsync(command, _ => RedirectToAction("Index"));

    [HttpPost]
    public async Task<IActionResult> UpdateItem(UpdateItemCommand command) =>
        await CallHandlerAsync(command, _ => RedirectToAction("Index"));

    [HttpPost]
    public async Task<IActionResult> DeleteItem(int id) =>
        await CallHandlerAsync(new DeleteItemCommand(id), _ => RedirectToAction("Index"));

    [HttpPost]
    public async Task<IActionResult> UpsertTelegramItem(UpsertTelegramItemCommand command) =>
        await CallHandlerAsync(command, _ => RedirectToAction("Index"));
}