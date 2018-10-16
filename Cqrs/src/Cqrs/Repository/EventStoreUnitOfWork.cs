namespace SoderbergPartners.Kalle.Cqrs.Repository
{
    /// <summary>
    /// A unit of work working against an event store.
    /// </summary>
    public class EventStoreUnitOfWork : IUnitOfWork
    {
        private readonly AggregateTracker aggregateTracker;
        private readonly IEventStore eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreUnitOfWork"/> class.
        /// </summary>
        /// <param name="eventStore">The event store.</param>
        /// <param name="aggregateTracker">The aggregate tracker.</param>
        public EventStoreUnitOfWork(IEventStore eventStore, AggregateTracker aggregateTracker)
        {
            this.eventStore = eventStore;
            this.aggregateTracker = aggregateTracker;
        }

        /// <summary>
        /// Commits the unit of work.
        /// </summary>
        public void Commit()
        {
            foreach (var aggregate in aggregateTracker.TrackedAggregates)
            {
                eventStore.SaveDomainEvents(aggregate.AggregateId, aggregate.Version, aggregate.UncommittedDomainEvents);
                aggregate.ClearUncommittedDomainEvents();
            }

            aggregateTracker.ClearTrackedAggregates();
        }

        /// <summary>
        /// Rollbacks the unit of work.
        /// </summary>
        public void Rollback()
        {
            foreach (var aggregate in aggregateTracker.TrackedAggregates)
            {
                aggregate.ClearUncommittedDomainEvents();
            }

            aggregateTracker.ClearTrackedAggregates();
        }
    }
}
