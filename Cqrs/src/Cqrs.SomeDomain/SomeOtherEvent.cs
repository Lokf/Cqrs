using Lokf.Cqrs.Domain;
using System;

namespace Lokf.Cqrs.SomeDomain
{
    public class SomeOtherEvent : DomainEvent
    {
        public SomeOtherEvent(Guid eventId, int value)
            : base(eventId)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
