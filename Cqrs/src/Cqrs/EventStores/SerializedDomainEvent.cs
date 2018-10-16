using System;

namespace SoderbergPartners.Kalle.Cqrs.EventStores
{
    /// <summary>
    /// The serialized presentation of a domain event.
    /// </summary>
    public sealed class SerializedDomainEvent
    {
        /// <summary>
        /// Gets or sets the aggregate ID.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets aggregate version.
        /// </summary>
        public int AggregateVersion { get; set; }

        /// <summary>
        /// Gets or sets the event ID.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public string Payload { get; set; }
    }
}
