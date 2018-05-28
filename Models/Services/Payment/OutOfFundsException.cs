using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.Services.Payment
{
    [Serializable]
    public class OutOfFundsException : Exception
    {
        public OutOfFundsException() { }
        public OutOfFundsException(string message) : base(message) { }
        public OutOfFundsException(string message, Exception inner) : base(message, inner) { }
        protected OutOfFundsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
