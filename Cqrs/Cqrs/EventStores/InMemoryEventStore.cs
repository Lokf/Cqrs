using Lokf.Cqrs.Aggregates;
using Lokf.Cqrs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lokf.Cqrs.EventStores
{
    /// <summary>
    /// An event store that purely exists in memory.
    /// </summary>
    public class InMemoryEventStore : IEventStore
    {
        /// <summary>
        /// The domain events that have happend since the event store was loaded.
        /// </summary>
        private readonly List<IDomainEvent> changes = new List<IDomainEvent>();

        /// <summary>
        /// The event store.
        /// </summary>
        private readonly Dictionary<Guid, List<IDomainEvent>> eventStore = new Dictionary<Guid, List<IDomainEvent>>();

        /// <summary>
        /// Sets the history. The events are loaded into the event store for setting up the state of the application.
        /// </summary>
        /// <param name="history">The history of domain events.</param>
        public void SetHistory(IEnumerable<IDomainEvent> history)
        {
            foreach (var domainEvent in history)
            {
                if (eventStore.TryGetValue(domainEvent.AggregateId, out var stream))
                {
                    stream.Add(domainEvent);
                }
                else
                {
                    stream = new List<IDomainEvent> { domainEvent };
                    eventStore.Add(domainEvent.AggregateId, stream);
                }
            }
        }

        /// <summary>
        /// Gets the domain events for the specified aggregate ID.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>The domain events.</returns>
        public IEnumerable<IDomainEvent> GetDomainEvents(Guid aggregateId)
        {
            if (eventStore.TryGetValue(aggregateId, out var stream)) {
                return stream;
            }

            return Enumerable.Empty<IDomainEvent>();
        }

        /// <summary>
        /// Saves the domain events for the aggregate.
        /// </summary>
        /// <exception cref="OptimisticConcurrencyException">
        /// Thrown when the expected version of the aggregate does not match the actual version in the event store.
        /// </exception>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <param name="domainEvents">The domain events.</param>
        /// <param name="expectedVersion">The expected version.</param>
        public void SaveDomainEvents(Guid aggregateId, IEnumerable<IDomainEvent> domainEvents, int expectedVersion)
        {
            if (eventStore.TryGetValue(aggregateId, out var stream))
            {
                VerifyVersion(expectedVersion, stream.Count, aggregateId);

                stream.AddRange(domainEvents);
            }
            else
            {
                VerifyVersion(expectedVersion, 0, aggregateId);

                stream = new List<IDomainEvent>(domainEvents);
                eventStore.Add(aggregateId, stream);
            }

            changes.AddRange(domainEvents);
        }

        /// <summary>
        /// Gets the domain events that has been saved since the event store was loaded.
        /// </summary>
        /// <returns>The new domain events.</returns>
        public IEnumerable<IDomainEvent> PeekChanges()
        {
            return changes;
        }

        private void VerifyVersion(int expectedVersion, int actualVersion, Guid aggregateId)
        {
            if (expectedVersion != actualVersion)
            {
                var message = $"Expected version: {expectedVersion} of aggregate: {aggregateId}, but the actual version was: {actualVersion}";
                throw new OptimisticConcurrencyException(message);
            }
        }
    }
}
