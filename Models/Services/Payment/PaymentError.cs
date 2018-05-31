using Newtonsoft.Json;

namespace FiveDevsShop.Models.Services.Payment
{
    public class PaymentError
    {
        [JsonProperty(PropertyName = "error")]
        public string ErrorKind { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
