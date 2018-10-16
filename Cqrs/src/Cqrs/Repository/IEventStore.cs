using Lokf.Cqrs.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Repository
{
    /// <summary>
    /// An event store.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Gets the domain events associated with the aggregate.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>The domain events.</returns>
        IEnumerable<IDomainEvent> GetDomainEvents(Guid aggregateId);

        /// <summary>
        /// Saves the domain events.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <param name="expectedVersion">The expected version.</param>
        /// /// <param name="domainEvents">The domain events.</param>
        void SaveDomainEvents(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> domainEvents);
    }
}
