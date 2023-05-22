using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aurora.Domain.Shared.Extensions; 

public static class JsonExtensions {
    public static string ToJsonString(this object self) {
        return JsonSerializer.Serialize(self, new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }
}