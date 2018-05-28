using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.Services.Payment
{
    public class PaymentProcessor
    {
        private const string PaymentUrl = "";
        private readonly HttpClient httpClient;

        public PaymentProcessor(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PaymentResponse> Pay(PaymentData data)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(PaymentUrl),
                Method = HttpMethod.Post,
                // TODO: this is ba64 encoded "technologines:platformos", don't hardcode this?
                Headers = { { HttpRequestHeader.Authorization.ToString(), "Basic dGVjaG5vbG9naW5lczpwbGF0Zm9ybW9z" } },
                Content = MakeContent(data),
            };

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            switch ((int)response.StatusCode)
            {
                case 201:
                    return JsonConvert.DeserializeObject<PaymentResponse>(content);
                case 401:
                    // authentification fail, shouldn't happen because we provide known username+password
                    throw new UnknownErrorException("Authentification error");
                case 402:
                case 400:
                    var error = ParseError(content);
                    if (error.ErrorKind == "ValidationError")
                        throw new ValidationException(error.Message);
                    else if (error.ErrorKind == "OutOfFunds")
                        throw new OutOfFundsException();
                    else if (error.ErrorKind == "CardExpired")
                        throw new CardExpiredException();
                    else
                        throw new UnknownErrorException(error.Message);
                default:
                    throw new UnknownErrorException();
            }
        }

        private HttpContent MakeContent(PaymentData data)
        {
            var content = JsonConvert.SerializeObject(data);
            return new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        }

        private PaymentError ParseError(string json)
        {
            return JsonConvert.DeserializeObject<PaymentError>(json);
        }
    }
}
