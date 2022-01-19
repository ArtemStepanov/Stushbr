using Stushbr.Shared.Configuration;

namespace Stushbr.EntitiesProcessor.Configuration;

public class ProcessorConfiguration : ApplicationConfiguration
{
    public TelegramConfiguration Telegram { get; set; }

    public SendPulseConfiguration SendPulse { get; set; }
}
