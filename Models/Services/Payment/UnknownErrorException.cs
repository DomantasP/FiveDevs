using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.Services.Payment
{
    [Serializable]
    public class UnknownErrorException : Exception
    {
        public UnknownErrorException() { }
        public UnknownErrorException(string message) : base(message) { }
        public UnknownErrorException(string message, Exception inner) : base(message, inner) { }
        protected UnknownErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
