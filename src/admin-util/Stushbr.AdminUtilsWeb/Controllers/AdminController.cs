using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Controllers;

[Authorize(Policy = "Admin")]
public class AdminController : Controller
{
    private readonly StushbrDbContext _dbContext;

    public AdminController(StushbrDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MigrateAsync()
    {
        await _dbContext.Database.MigrateAsync(HttpContext.RequestAborted);
        return RedirectToAction("Index");
    }
}