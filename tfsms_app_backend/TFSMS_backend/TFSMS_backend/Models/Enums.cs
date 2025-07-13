using System.Text.Json.Serialization;

namespace TFSMS_app_backend.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransportMethodType
    {
        ByOwn,
        ByCollector
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethodType
    {
        Cash,
        Bank
    }
}
