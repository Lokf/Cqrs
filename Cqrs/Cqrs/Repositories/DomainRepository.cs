using Lokf.Cqrs.Aggregates;
using System;

namespace Lokf.Cqrs.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IEventStore eventStore;

        public DomainRepository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public void Add<TAggregate>(TAggregate aggregate) 
            where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }

        public TAggregate GetAggregate<TAggregate>(Guid aggregateId) 
            where TAggregate : AggregateRoot
        {
            var aggregate = AggregateFactory<TAggregate>.Create(aggregateId);

            var history = eventStore.GetDomainEvents(aggregateId);
            aggregate.LoadFromHistory(history);

            return aggregate;
        }
    }
}
