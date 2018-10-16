using SoderbergPartners.Kalle.Cqrs.Domain;
using System;

namespace SoderbergPartners.Kalle.Cqrs.SomeDomain
{
    public class SomeAggregate : AggregateRoot
    {
        public SomeAggregate(Guid aggregateId)
            : base(aggregateId)
        {
            AddStateTransition<SomeEvent>(OnSomeEvent);
            AddStateTransition<SomeOtherEvent>(OnSomeOtherEvent);
        }

        public Guid EventId { get; private set; } = Guid.Empty;

        public int Value { get; private set; } = 0;

        public void RaiseSomeEvent(Guid eventId)
        {
            var someEvent = new SomeEvent(eventId);
            RaiseDomainEvent(someEvent);
        }

        public void RaiseSomeOtherEvent(Guid eventId, int value)
        {
            var someOtherEvent = new SomeOtherEvent(eventId, value);
            RaiseDomainEvent(someOtherEvent);
        }

        private void OnSomeEvent(SomeEvent someEvent)
        {
            EventId = someEvent.EventId;
        }

        private void OnSomeOtherEvent(SomeOtherEvent someOtherEvent)
        {
            Value += someOtherEvent.Value;
        }
    }
}
