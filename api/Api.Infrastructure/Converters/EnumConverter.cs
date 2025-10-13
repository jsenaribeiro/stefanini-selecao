using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EnumConverter<T> : JsonConverter<T?> where T : struct, Enum
{
	public override T? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions jso) =>
		 Enum.TryParse<T>(reader.GetString(), true, out var value) ? value : null;

	public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions jso)
	{
		if (value.HasValue == false) writer.WriteNullValue();
		else writer.WriteStringValue(value.Value.ToString());
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class JsonEnumAttribute<T> : JsonConverterAttribute where T : Enum
{
	public JsonEnumAttribute() : base(typeof(T)) { }
}