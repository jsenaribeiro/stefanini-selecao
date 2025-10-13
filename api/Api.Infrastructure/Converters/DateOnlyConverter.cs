using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private readonly string _format = "dd/MM/yyyy";

    public override DateOnly Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions jso) =>
        DateOnly.ParseExact(reader.GetString()!, _format, CultureInfo.InvariantCulture);

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions jso) =>
        writer.WriteStringValue(value.ToString(_format));
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class JsonDateOnlyAttribute : JsonConverterAttribute
{
   public JsonDateOnlyAttribute() : base(typeof(DateOnly)) { }  
}