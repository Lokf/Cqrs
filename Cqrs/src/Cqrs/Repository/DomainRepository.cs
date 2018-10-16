using System;
using SoderbergPartners.Kalle.Cqrs.Domain;

namespace SoderbergPartners.Kalle.Cqrs.Repository
{
    /// <summary>
    /// An domain repository backed by an event store.
    /// </summary>
    public sealed class DomainRepository : IDomainRepository
    {
        private readonly IEventStore eventStore;
        private readonly AggregateTracker aggregateTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRepository"/> class.
        /// </summary>
        /// <param name="eventStore">The event store.</param>
        /// <param name="aggregateTracker">The aggregate tracker.</param>
        public DomainRepository(IEventStore eventStore, AggregateTracker aggregateTracker)
        {
            this.eventStore = eventStore;
            this.aggregateTracker = aggregateTracker;
        }

        /// <summary>
        /// Adds an aggregate to the repository.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregate">The aggregate.</param>
        public void Add<TAggregate>(TAggregate aggregate)
            where TAggregate : AggregateRoot
        {
            aggregateTracker.Track(aggregate);
        }

        /// <summary>
        /// Gets the aggregate with the given ID.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregateId">The ID of the aggregate.</param>
        /// <returns>Returns the aggregate with the ID</returns>
        public TAggregate GetAggregate<TAggregate>(Guid aggregateId)
            where TAggregate : AggregateRoot
        {
            var aggregate = AggregateFactory<TAggregate>.Create(aggregateId);

            var history = eventStore.GetDomainEvents(aggregateId);
            aggregate.LoadFromHistory(history);

            aggregateTracker.Track(aggregate);

            return aggregate;
        }
    }
}
