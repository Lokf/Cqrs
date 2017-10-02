using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Aggregates
{
    public interface IDomainEvent
    {
        Guid AggregateId { get; }
    }
}
