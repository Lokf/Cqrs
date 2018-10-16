using System;
using System.Runtime.Serialization;

namespace Lokf.Cqrs.Repository
{
    /// <summary>
    /// Exception thrown when a concurreny issue is detected when saving events.
    /// </summary>
    [Serializable]
    public class OptimisticConcurrencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticConcurrencyException"/> class.
        /// </summary>
        public OptimisticConcurrencyException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticConcurrencyException"/> class with the specified message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public OptimisticConcurrencyException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public OptimisticConcurrencyException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticConcurrencyException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected OptimisticConcurrencyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
