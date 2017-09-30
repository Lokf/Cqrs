using System;
using System.Collections.Generic;
using System.Text;
using Lokf.Cqrs.Aggregates;

namespace Lokf.Cqrs.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IEventStore eventStore;

        public DomainRepository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public void Add<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }

        public TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}
