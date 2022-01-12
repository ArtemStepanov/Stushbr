using Qiwi.BillPayments.Client;
using Stushbr.PaymentsGatewayWeb.Configuration;
using Stushbr.PaymentsGatewayWeb.MapperProfiles;
using Stushbr.PaymentsGatewayWeb.Repositories;
using Stushbr.PaymentsGatewayWeb.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration.Get<ApplicationConfiguration>();

// Add services to the container.
services.AddAutoMapper(typeof(ItemProfile));

services.AddSingleton(BillPaymentsClientFactory.Create(configuration.Qiwi.SecretToken));
services.AddSingleton<IQiwiService, QiwiService>();

services.AddTransient<IItemService, ItemService>();
services.AddTransient<IItemRepository, ItemRepository>();
services.AddTransient<IClientRepository, ClientRepository>();

services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
