using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stushbr.AdminUtilsWeb.Domain.Contracts;
using Stushbr.Application.ExceptionHandling;

namespace Stushbr.AdminUtilsWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var errorModel = new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier, EvaluateMessage());

        return View(errorModel);

        string? EvaluateMessage()
        {
            if (exceptionHandlerPathFeature is null)
            {
                return null;
            }

            return exceptionHandlerPathFeature.Error switch
            {
                BadRequestException badRequestException => badRequestException.Message,
                NotFoundException notFoundException => notFoundException.Message,
                _ => null
            };
        }
    }

    [HttpPost]
    public IActionResult SignOutAction()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}