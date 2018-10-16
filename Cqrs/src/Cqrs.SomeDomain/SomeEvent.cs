using Lokf.Cqrs.Domain;
using System;

namespace Lokf.Cqrs.SomeDomain
{
    public class SomeEvent : DomainEvent
    {
        public SomeEvent(Guid eventId)
            : base(eventId)
        {
        }
    }
}
