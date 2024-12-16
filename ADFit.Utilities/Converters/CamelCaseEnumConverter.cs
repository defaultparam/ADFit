using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ADFit.Utilities.Converters;
public class CamelCaseEnumConverter : JsonConverter
{
    private readonly NamingStrategy _namingStrategy;

    public CamelCaseEnumConverter()
    {
        _namingStrategy = new CamelCaseNamingStrategy();
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsEnum;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is Enum enumValue)
        {
            var camelCaseName = _namingStrategy.GetPropertyName(enumValue.ToString(), false);
            writer.WriteValue(camelCaseName);
        }
        else
        {
            throw new JsonSerializationException("Value is not an Enum.");
        }
    }
}