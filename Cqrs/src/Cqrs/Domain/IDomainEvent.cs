using System;

namespace Lokf.Cqrs.Domain
{
    /// <summary>
    /// A domain event is raised to represent that something has happend inside an aggregate.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the event ID.
        /// </summary>
        Guid EventId { get; }
    }
}
