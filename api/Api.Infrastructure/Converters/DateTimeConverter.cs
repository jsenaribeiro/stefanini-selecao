using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format = "dd/MM/yyyy";

    public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions jso) =>
        DateTime.ParseExact(reader.GetString()!, _format, CultureInfo.InvariantCulture);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions jso) =>
        writer.WriteStringValue(value.ToString(_format));
}