using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stushbr.Core.Configuration;
using Stushbr.Function.Payment;
using Stushbr.Function.Payment.Configurations;
using Stushbr.Function.Payment.HttpClients;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Stushbr.Function.Payment;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.Configure<SendPulseConfiguration>(builder.GetContext().Configuration.GetSection("SendPulse").Bind);
        builder.Services.Configure<TelegramConfiguration>(builder.GetContext().Configuration.GetSection("Telegram").Bind);

        builder.Services.AddHttpClient<SendPulseWebHookClient>((provider, opt) =>
        {
            opt.BaseAddress = new Uri(provider.GetRequiredService<IOptions<SendPulseConfiguration>>().Value.HookUrl);
            opt.DefaultRequestHeaders.Add("Accept", "application/json");
            opt.DefaultRequestHeaders.Add("User-Agent", GetType().Assembly.GetName().Name);
        });
    }
}