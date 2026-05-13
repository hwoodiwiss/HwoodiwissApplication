using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace HwoodiwissSyncer.Extensions;

public static class JsonSerializerOptionExtensions
{
#if NET11_0_OR_GREATER
    [Obsolete("This method is not needed in .NET 11 and later. Use JsonSerializerOptions.GetTypeInfo<T>() instead.")]
#endif
    public static JsonTypeInfo<T> GetJsonTypeInfo<T>(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

#if NET11_0_OR_GREATER
        return options.GetTypeInfo<T>();
#else
        if (options.GetTypeInfo(typeof(T)) is JsonTypeInfo<T> jsonTypeInfo)
        {
            return jsonTypeInfo;
        }

        throw new ArgumentException($"Unable to find JsonTypeInfo for {typeof(T).FullName}");
#endif
    }
}
