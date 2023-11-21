namespace Stushbr.AdminUtilsWeb.ViewModels.Items;

public sealed class CreateItemViewModel
{
    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public double Price { get; set; }

    public string ItemIdentifier { get; set; } = default!;

    public string Type { get; set; } = default!;

    public DateTime AvailableSince { get; set; } = DateTime.Now;

    public DateTime AvailableBefore { get; set; } = DateTime.Now.AddDays(10);
}