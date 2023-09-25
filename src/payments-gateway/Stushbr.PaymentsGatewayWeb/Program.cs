using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qiwi.BillPayments.Client;
using Stushbr.Api.ExceptionHandling;
using Stushbr.Api.Extensions;
using Stushbr.Application.Abstractions;
using Stushbr.Application.Services;
using Stushbr.Core.Configuration;
using Stushbr.Data.DataAccess.Postgres;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEssentials(builder.Configuration);
services.AddSwaggerGen();

#region Services

services.AddSingleton(provider => BillPaymentsClientFactory.Create(
        provider.GetRequiredService<ApplicationConfiguration>().Qiwi!.SecretToken
    )
);
services.AddSingleton<IQiwiService, QiwiService>();
services.AddScoped<IItemService, ItemService>();
services.AddScoped<IClientService, ClientService>();
services.AddScoped<IClientItemService, ClientItemService>();

#endregion

// Add services to the container.

services
    .AddControllersWithViews(opt => { opt.Filters.Add<HttpResponseExceptionFilter>(); })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var httpContext = context.HttpContext;
            var logger = httpContext.RequestServices
                .GetRequiredService<ILogger<HttpResponseExceptionFilter>>();

            var errorsResponse = new BadRequestException(context.ModelState).Response;

            logger.LogWarning(
                "Handling HttpException (BadRequestException).\n" +
                "Request: {RequestMethod} {RequestPath}\n" +
                "Message: {Message}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                errorsResponse.ToString()
            );

            return new JsonResult(errorsResponse) { StatusCode = 400 };
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    await using var serviceScope = app.Services.CreateAsyncScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<StushbrDbContext>();
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();