using Lokf.Cqrs.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Repositories
{
    public interface IEventStore
    {
        IEnumerable<IDomainEvent> GetDomainEvents(Guid aggregateId);

        void SaveDomainEvents(Guid aggregateId, IEnumerable<IDomainEvent> domainEvents, int expectedVersion);
    }
}
