using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stushbr.AdminUtilsWeb.Attributes;
using Stushbr.Api.Abstractions;
using Stushbr.Application.Commands.Service;

namespace Stushbr.AdminUtilsWeb.Controllers;

[AuthorizeAdmin]
public class AdminController(ISender sender) : StushbrControllerBase(sender)
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MigrateAsync() =>
        await CallHandlerAsync(new MigrateCommand(), result =>
        {
            if (!result.Success)
            {
                ModelState.AddModelError("Migrate", result.Message);
                return View("Index");
            }

            ViewData["MigrateResult"] = true;
            return View("Index");
        });
}