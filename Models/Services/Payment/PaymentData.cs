using Newtonsoft.Json;

namespace FiveDevsShop.Models.Services.Payment
{
    public class PaymentData
    {
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string CardNumber { get; set; }

        [JsonProperty(PropertyName = "holder")]
        public string Holder { get; set; }

        [JsonProperty(PropertyName = "exp_year")]
        public int ExpirationYear { get; set; }

        [JsonProperty(PropertyName = "exp_month")]
        public int ExpirationMonth { get; set; }

        [JsonProperty(PropertyName = "ccv")]
        public string Ccv { get; set; }
    }
}
