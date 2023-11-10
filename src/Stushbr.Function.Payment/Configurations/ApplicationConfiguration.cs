namespace Stushbr.Function.Payment.Configurations;

public sealed class ApplicationConfiguration
{
    public bool MigrationMode { get; set; }

    public string TildaWebhookSecret { get; set; } = default!;
}