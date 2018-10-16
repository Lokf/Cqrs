using Lokf.Cqrs.Domain;
using Lokf.Cqrs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lokf.Cqrs.EventStores
{
    /// <summary>
    /// An event store that can store and load domain events.
    /// </summary>
    public class EventStore : IEventStore
    {
        private readonly IEventPersistence eventPersistence;
        private readonly DomainEventSerializer domainEventSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStore"/> class.
        /// </summary>
        /// <param name="eventPersistence">The event persistence.</param>
        /// <param name="domainEventSerializer">The domain event serializer.</param>
        public EventStore(IEventPersistence eventPersistence, DomainEventSerializer domainEventSerializer)
        {
            this.eventPersistence = eventPersistence;
            this.domainEventSerializer = domainEventSerializer;
        }

        /// <summary>
        /// Gets the domain events for the given aggregate.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>The domain events.</returns>
        public IEnumerable<IDomainEvent> GetDomainEvents(Guid aggregateId)
        {
            return eventPersistence
                 .GetEventsByAggregateId(aggregateId)
                 .Select(domainEventSerializer.Deserialize);
        }

        /// <summary>
        /// Saves the domain events to the given aggregate.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <param name="expectedVersion">The expected aggregate version.</param>
        /// <param name="domainEvents">The domain events.</param>
        public void SaveDomainEvents(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> domainEvents)
        {
            var serializedEvents = new List<SerializedDomainEvent>();
            var version = expectedVersion;
            foreach (var domainEvent in domainEvents)
            {
                var metadata = new Metadata
                {
                    { MetadataKeys.EventName, domainEvent.GetType().FullName },
                };

                serializedEvents.Add(domainEventSerializer.Serialize(aggregateId, ++version, domainEvent, metadata));
            }

            eventPersistence.PersistEvents(aggregateId, expectedVersion, serializedEvents);
        }
    }
}
