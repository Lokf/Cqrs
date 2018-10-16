using SoderbergPartners.Kalle.Cqrs.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoderbergPartners.Kalle.Cqrs.EventStores
{
    /// <summary>
    /// Event persistence.
    /// </summary>
    public interface IEventPersistence
    {
        /// <summary>
        /// Gets the events that are persistet to the given aggregate ID.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>The events.</returns>
        IReadOnlyCollection<SerializedDomainEvent> GetEventsByAggregateId(Guid aggregateId);

        /// <summary>
        /// Persits the events.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID </param>
        /// <param name="expectedVersion">The expected aggregate version.</param>
        /// <param name="serializedDomainEvents">The serialized domain events.</param>
        void PersistEvents(Guid aggregateId, int expectedVersion, IEnumerable<SerializedDomainEvent> serializedDomainEvents);
    }
}
