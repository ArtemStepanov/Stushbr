using Stushbr.Shared.Models;

namespace Stushbr.EntitiesProcessor.Services;

public interface IItemProcessorService
{
    Task ProcessItemAsync(Bill billItem, CancellationToken cancellationToken);
}
