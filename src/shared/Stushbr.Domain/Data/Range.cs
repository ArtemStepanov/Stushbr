namespace Stushbr.Domain.Data;

public sealed class Range<T> where T : class
{
    public int TotalCount { get; set; }

    public T[] Items { get; set; }

    public Range()
    {
        Items = Array.Empty<T>();
    }
}