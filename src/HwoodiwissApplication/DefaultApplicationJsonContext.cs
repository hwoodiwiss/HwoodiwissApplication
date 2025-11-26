using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Hwoodiwiss.Extensions.Hosting;

[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(KeyValuePair<string, string>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
internal sealed partial class DefaultApplicationJsonContext : JsonSerializerContext;

