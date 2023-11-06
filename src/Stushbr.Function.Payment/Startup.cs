using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stushbr.Api.Extensions;
using Stushbr.Core.Configuration;
using Stushbr.Function.Payment;
using Stushbr.Function.Payment.Configurations;
using Stushbr.Function.Payment.HttpClients;
using System;
using ApplicationConfiguration = Stushbr.Function.Payment.Configurations.ApplicationConfiguration;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Stushbr.Function.Payment;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddEssentials(builder.GetContext().Configuration);

        builder.Services.Configure<SendPulseConfiguration>(builder.GetContext().Configuration.GetSection("SendPulse").Bind);
        builder.Services.Configure<TelegramConfiguration>(builder.GetContext().Configuration.GetSection("Telegram").Bind);
        builder.Services.Configure<TildaConfiguration>(builder.GetContext().Configuration.GetSection("Tilda").Bind);
        builder.Services.Configure<ApplicationConfiguration>(builder.GetContext().Configuration.GetSection("Application").Bind);

        builder.Services.AddHttpClient<SendPulseWebHookClient>((provider, opt) =>
        {
            opt.BaseAddress = new Uri(provider.GetRequiredService<IOptions<SendPulseConfiguration>>().Value.HookUrl);
            opt.DefaultRequestHeaders.Add("Accept", "application/json");
            opt.DefaultRequestHeaders.Add("User-Agent", GetType().Assembly.GetName().Name);
        });
    }
}