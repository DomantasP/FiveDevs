using Newtonsoft.Json;

namespace FiveDevsShop.Models.Services.Payment
{
    public class PaymentResponse : PaymentData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public string PaymentDate { get; set; }
    }
}
