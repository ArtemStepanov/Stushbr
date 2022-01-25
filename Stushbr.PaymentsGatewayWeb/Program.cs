using Microsoft.AspNetCore.Mvc;
using Stushbr.Shared.Configuration;
using Stushbr.Shared.ExceptionHandling;
using Stushbr.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var configuration = builder.Configuration.Get<ApplicationConfiguration>();

services.AddEssentials(configuration);
services.AddSwaggerGen();

// Add services to the container.

services
    .AddControllersWithViews(opt => { opt.Filters.Add<HttpResponseExceptionFilter>(); })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(new ErrorsResponse("Неверное состояние запроса"));
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
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
