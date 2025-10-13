using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string _format = "HH:mm:ss";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions jso) =>
        TimeOnly.ParseExact(reader.GetString()!, _format, CultureInfo.InvariantCulture);

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions jso) =>
        writer.WriteStringValue(value.ToString(_format));
}