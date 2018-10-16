using SoderbergPartners.Kalle.Cqrs.Domain;
using System;

namespace SoderbergPartners.Kalle.Cqrs.SomeDomain
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
