using SoderbergPartners.Kalle.Cqrs.Domain;
using System;

namespace SoderbergPartners.Kalle.Cqrs.SomeDomain
{
    public class SomeEvent : DomainEvent
    {
        public SomeEvent(Guid eventId)
            : base(eventId)
        {
        }
    }
}
