using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RssApp.Application.Services.AppSettings;

public class AppSettings : IAppSettings
{
    private const string ConfigPath = "config.json";

    private Dictionary<string, object> _values = [];

    public string? SqlConnectionString => GetValue<string>();

    public async Task InitAsync()
    {
        string fileContent = await File.ReadAllTextAsync(ConfigPath);
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContent);
        _values = values ?? throw new FileLoadException("");
    }

    private T? GetValue<T>([CallerMemberName] string? propertyName = null) where T : class
    {
        return GetValue(propertyName) as T
               ?? throw new InvalidCastException($"{propertyName} cannot be treated as {typeof(T).Name}");
    }

    private object? GetValue(string? propertyName)
    {
        return _values.GetValueOrDefault(GetJsonPropertyName(propertyName));
    }

    private string GetJsonPropertyName(string? key)
    {
        PropertyInfo[] properties = GetType().GetProperties();

        if (properties.All(x => x.Name != key))
        {
            throw new ArgumentOutOfRangeException($"Property \"{key}\" was not found in \"{nameof(AppSettings)}\"");
        }

        PropertyInfo property = properties.Single(x => x.Name == key);
        object[] attributes = property.GetCustomAttributes(true);

        return attributes.Count(x => x is JsonPropertyNameAttribute) switch
        {
            0 => key,
            1 => (attributes.Single(x => x is JsonPropertyNameAttribute) as JsonPropertyNameAttribute)!.Name,
            _ => throw new Exception(
                $"Could not determine JSON property name for \"{key}\" as it has multiple {nameof(JsonPropertyNameAttribute)}s")
        } ?? string.Empty;
    }
}
