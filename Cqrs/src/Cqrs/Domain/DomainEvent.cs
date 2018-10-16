using System;

namespace SoderbergPartners.Kalle.Cqrs.Domain
{
    /// <summary>
    /// Base class for all domain events.
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent"/> class.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        public DomainEvent(Guid eventId)
        {
            EventId = eventId;
        }

        /// <summary>
        /// Gets the event ID.
        /// </summary>
        public Guid EventId { get; }
    }
}
