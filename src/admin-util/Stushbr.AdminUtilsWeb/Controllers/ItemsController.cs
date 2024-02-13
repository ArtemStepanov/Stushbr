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
    public Task<IActionResult> Index() =>
        CallHandlerAsync(new GetItemsQuery(), View);

    [HttpPost]
    public Task<IActionResult> AddItem(CreateItemCommand command) =>
        CallHandlerAsync(command, _ => RedirectToAction("Index"));

    [HttpPost]
    public Task<IActionResult> UpdateItem(UpdateItemCommand command) =>
        CallHandlerAsync(command, _ => RedirectToAction("Index"));

    [HttpPost]
    public Task<IActionResult> DeleteItem(int id) =>
        CallHandlerAsync(new DeleteItemCommand(id), _ => RedirectToAction("Index"));

    [HttpPost]
    public Task<IActionResult> UpsertTelegramItem(UpsertTelegramItemCommand command) =>
        CallHandlerAsync(command, _ => RedirectToAction("Index"));
}