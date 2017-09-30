using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Aggregates
{
    public abstract class AggregateRoot
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> stateChangers = new Dictionary<Type, Action<IDomainEvent>>();

        private readonly List<IDomainEvent> uncommitedDominEvents = new List<IDomainEvent>();

        public Guid AggregateId { get; }

        public AggregateRoot(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public IEnumerable<IDomainEvent> UncommittedDomainEvents => uncommitedDominEvents;

        public int Version { get; private set; }

        public void ClearUncommittedDomainEvents()
        {
            uncommitedDominEvents.Clear();
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> history)
        {
            foreach (var stateChange in history)
            {
                ApplyDomainEvent(stateChange);
            }
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            ApplyDomainEvent(domainEvent);

            uncommitedDominEvents.Add(domainEvent);            
        }

        private void ApplyDomainEvent(IDomainEvent domainEvent)
        {
            var type = domainEvent.GetType();

            var stateChanger = stateChangers[type];

            stateChanger(domainEvent);
        }
    }
}
