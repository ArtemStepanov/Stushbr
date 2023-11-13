using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Controllers;

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