using System;

namespace Lokf.Cqrs.Aggregates
{
    /// <summary>
    /// Base class for all domain events.
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initiliazes a new instance of the <cref="DomainEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            DomainEventId = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the domain event ID.
        /// </summary>
        public Guid DomainEventId { get; }

        /// <summary>
        /// Gets the aggregate ID.
        /// </summary>
        public Guid AggregateId { get; }
    }
}
