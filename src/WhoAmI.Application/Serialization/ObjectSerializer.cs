using System.Text.Json;
using WhoAmI.Application.Abstractions.Serialization;

namespace WhoAmI.Application.Serialization;

internal class ObjectSerializer : IObjectSerializer {
    private readonly JsonSerializerOptions _options;

    public ObjectSerializer(JsonSerializerOptions? options = null) =>
        _options = options ?? new JsonSerializerOptions();

    public string Serialize(object value) =>
        JsonSerializer.Serialize(value, _options);
}
