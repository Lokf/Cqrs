using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lokf.Cqrs.EventStores
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException()
        {
        }

        public OptimisticConcurrencyException(string message) : base(message)
        {
        }

        public OptimisticConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OptimisticConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
