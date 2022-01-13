using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Qiwi.BillPayments.Client;
using Stushbr.PaymentsGatewayWeb.Configuration;
using Stushbr.PaymentsGatewayWeb.MapperProfiles;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.Repositories;
using Stushbr.PaymentsGatewayWeb.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration.Get<ApplicationConfiguration>();

services.AddSwaggerGen();
services.AddSingleton(configuration);

// Add services to the container.

services.AddAutoMapper(typeof(ItemProfile), typeof(ClientProfile));
services.AddSingleton(BillPaymentsClientFactory.Create(configuration.Qiwi!.SecretToken));

#region Repositories

services.AddLinqToDbContext<StushbrDataConnection>((provider, options) =>
{
    options
        .UsePostgreSQL(builder.Configuration.GetConnectionString("Postgre"))
        .UseDefaultLogging(provider);
});

#endregion

#region Services

services.AddSingleton<IQiwiService, QiwiService>();
services.AddTransient<IItemService, ItemService>();
services.AddTransient<IClientService, ClientService>();
services.AddTransient<IBillService, BillService>();

#endregion

services.AddControllersWithViews();

var app = builder.Build();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var serviceScope = serviceScopeFactory.CreateScope())
{
    var dataConnection = serviceScope.ServiceProvider.GetRequiredService<StushbrDataConnection>();
    dataConnection.CreateTable<Item>(tableOptions: TableOptions.CreateIfNotExists);
    dataConnection.CreateTable<Bill>(tableOptions: TableOptions.CreateIfNotExists);
    dataConnection.CreateTable<Client>(tableOptions: TableOptions.CreateIfNotExists);
}

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
