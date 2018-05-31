using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.Services.Payment
{
    public interface IPaymentProcessor
    {
        Task<PaymentResponse> Pay(PaymentData data);
    }
}
