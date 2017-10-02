using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Aggregates
{
    /// <summary>
    /// Common interface for all domain events.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the aggregate ID.
        /// </summary>
        Guid AggregateId { get; }
    }
}
