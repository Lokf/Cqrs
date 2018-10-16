using Lokf.Cqrs.Domain;
using Lokf.Cqrs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lokf.Cqrs.Scenarios
{
    /// <summary>
    /// An implmentation of an event store only stored in memory.
    /// </summary>
    public sealed class InMemoryEventStore : IEventStore
    {
        private readonly List<IDomainEvent> changes = new List<IDomainEvent>();
        private readonly Dictionary<Guid, List<IDomainEvent>> eventStore = new Dictionary<Guid, List<IDomainEvent>>();

        /// <summary>
        /// Gets the domain events for the specified aggregate ID.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>The domain events.</returns>
        public IEnumerable<IDomainEvent> GetDomainEvents(Guid aggregateId)
        {
            if (eventStore.TryGetValue(aggregateId, out var stream))
            {
                return stream;
            }

            return Enumerable.Empty<IDomainEvent>();
        }

        /// <summary>
        /// Gets the domain events that has been saved since the event store was loaded.
        /// </summary>
        /// <returns>The new domain events.</returns>
        public IEnumerable<IDomainEvent> PeekChanges()
        {
            return changes;
        }

        /// <summary>
        /// Saves the domain events for the aggregate.
        /// </summary>
        /// <exception cref="OptimisticConcurrencyException">
        /// Thrown when the expected version of the aggregate does not match the actual version in the event store.
        /// </exception>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <param name="expectedVersion">The expected version.</param>
        /// <param name="domainEvents">The domain events.</param>
        public void SaveDomainEvents(Guid aggregateId, int expectedVersion, IEnumerable<IDomainEvent> domainEvents)
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
        /// Sets the history. The events are loaded into the event store for setting up the state of the application.
        /// </summary>
        /// <remarks>
        /// Any previous events in the event stora are removed before the history is set.
        /// </remarks>
        /// <param name="history">The history of domain events.</param>
        public void SetHistory(Dictionary<Guid, List<IDomainEvent>> history)
        {
            eventStore.Clear();

            foreach (var stream in history)
            {
                eventStore[stream.Key] = new List<IDomainEvent>(stream.Value);
            }
        }

        private static void VerifyVersion(int expectedVersion, int actualVersion, Guid aggregateId)
        {
            if (expectedVersion != actualVersion)
            {
                var message = $"Expected version: {expectedVersion} of aggregate: {aggregateId}, but the actual version was: {actualVersion}";
                throw new OptimisticConcurrencyException(message);
            }
        }
    }
}
