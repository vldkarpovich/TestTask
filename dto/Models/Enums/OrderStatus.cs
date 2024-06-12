using System.Text.Json.Serialization;

namespace DAL.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus : int { Created = 1, ReceiptCreated = 2, EmailSended = 3}
}
