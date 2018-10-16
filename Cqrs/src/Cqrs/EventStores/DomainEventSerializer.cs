using Newtonsoft.Json;
using SoderbergPartners.Kalle.Cqrs.Domain;
using System;

namespace SoderbergPartners.Kalle.Cqrs.EventStores
{
    /// <summary>
    /// A seriliazer for domain events.
    /// </summary>
    public sealed class DomainEventSerializer
    {
        private readonly TypeMapping typeMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventSerializer"/> class.
        /// </summary>
        /// <param name="typeMapping">The type mapping used for deserialization.</param>
        public DomainEventSerializer(TypeMapping typeMapping)
        {
            this.typeMapping = typeMapping;
        }

        /// <summary>
        /// Serializes a domain event.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <param name="aggregateVersion">The aggregate version.</param>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The the serialized domain event.</returns>
        public SerializedDomainEvent Serialize(Guid aggregateId, int aggregateVersion, IDomainEvent domainEvent, Metadata metadata)
        {
            return new SerializedDomainEvent
            {
                AggregateId = aggregateId,
                AggregateVersion = aggregateVersion,
                EventId = domainEvent.EventId,
                Payload = JsonConvert.SerializeObject(domainEvent),
                Metadata = JsonConvert.SerializeObject(metadata),
            };
        }

        /// <summary>
        /// Deseriliazes a domain event.
        /// </summary>
        /// <param name="serializedDomainEvent">The seriliazed domain event.</param>
        /// <returns>The deserialized domain event.</returns>
        public IDomainEvent Deserialize(SerializedDomainEvent serializedDomainEvent)
        {
            var metadata = JsonConvert.DeserializeObject<Metadata>(serializedDomainEvent.Metadata);
            var eventName = metadata.EventName;
            var eventType = typeMapping.GetType(eventName);
            return (IDomainEvent)JsonConvert.DeserializeObject(serializedDomainEvent.Payload, eventType);
        }
    }
}
