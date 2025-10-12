using System.Text.Json.Serialization;

namespace Api.Infrastructure.Values;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Order { ASC = 0, DESC }