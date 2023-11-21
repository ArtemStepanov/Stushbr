using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Controllers;

[Authorize(Policy = "Admin")]
public class AdminController(StushbrDbContext dbContext) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MigrateAsync()
    {
        // todo: use mediatr
        await dbContext.Database.MigrateAsync(HttpContext.RequestAborted);
        return RedirectToAction("Index");
    }
}