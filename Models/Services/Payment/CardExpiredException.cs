using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.Services.Payment
{
    [Serializable]
    public class CardExpiredException : Exception
    {
        public CardExpiredException() { }
        public CardExpiredException(string message) : base(message) { }
        public CardExpiredException(string message, Exception inner) : base(message, inner) { }
        protected CardExpiredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
