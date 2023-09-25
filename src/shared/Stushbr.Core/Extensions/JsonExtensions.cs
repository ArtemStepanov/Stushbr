using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Core.Extensions;

public static class JsonExtensions
{
    public static JsonNode? JsonNodeFromObject(this object any)
    {
        return JsonSerializer.SerializeToNode(any);
    }

    public static T? ToObject<T>(this JsonNode json)
    {
        return json.Deserialize<T>();
    }
}
