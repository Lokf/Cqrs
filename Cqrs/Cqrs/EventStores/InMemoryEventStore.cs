using Lokf.Cqrs.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Lokf.Cqrs.Aggregates;
using System.Linq;

namespace Lokf.Cqrs.EventStores
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly List<IDomainEvent> changes = new List<IDomainEvent>();

        private readonly Dictionary<Guid, List<IDomainEvent>> eventStore = new Dictionary<Guid, List<IDomainEvent>>();

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

        public IEnumerable<IDomainEvent> GetDomainEvents(Guid aggregateId)
        {
            if (eventStore.TryGetValue(aggregateId, out var stream)) {
                return stream;
            }

            return Enumerable.Empty<IDomainEvent>();
        }

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
