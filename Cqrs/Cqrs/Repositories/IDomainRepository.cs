using Lokf.Cqrs.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Repositories
{
    public interface IDomainRepository
    {
        void Add<TAggregate>(TAggregate aggregate)
            where TAggregate : AggregateRoot;

        TAggregate GetAggregate<TAggregate>(Guid aggregateId)
            where TAggregate : AggregateRoot;
    }
}
