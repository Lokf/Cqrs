using System;
using System.Collections.Generic;
using System.Linq;

namespace Lokf.Cqrs.Domain
{
    /// <summary>
    /// Base class for all aggregate roots. Provides functionallity for raising domain event
    /// and rehydrating the aggregate from its history.
    /// </summary>
    public abstract class AggregateRoot
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> stateTransitions = new Dictionary<Type, Action<IDomainEvent>>();

        private readonly List<IDomainEvent> uncommitedDomainEvents = new List<IDomainEvent>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        public AggregateRoot(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        /// <summary>
        /// Gets the aggregate ID.
        /// </summary>
        public Guid AggregateId { get; }

        /// <summary>
        /// Gets the uncommitted domain events.
        /// </summary>
        public IEnumerable<IDomainEvent> UncommittedDomainEvents => uncommitedDomainEvents;

        /// <summary>
        /// Gets the current version of the aggregate, i.e. the number of events that has occurred since the beginning of the aggregate.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Clears the collection of uncommitted domain events.
        /// </summary>
        public void ClearUncommittedDomainEvents()
        {
            uncommitedDomainEvents.Clear();
        }

        /// <summary>
        /// Brings the aggregate inte its current state by replaying the history.
        /// </summary>
        /// <param name="history">The history of past domain events.</param>
        public void LoadFromHistory(IEnumerable<IDomainEvent> history)
        {
            foreach (var domainEvent in history)
            {
                ApplyDomainEvent(domainEvent);
                Version++;
            }
        }

        /// <summary>
        /// Adds a state transition for the domain event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event.</typeparam>
        /// <param name="stateTransition">The state transition.</param>
        protected void AddStateTransition<TDomainEvent>(Action<TDomainEvent> stateTransition)
            where TDomainEvent : IDomainEvent
        {
            stateTransitions.Add(typeof(TDomainEvent), domainEvent => stateTransition((TDomainEvent)domainEvent));
        }

        /// <summary>
        /// Raises a new domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            ApplyDomainEvent(domainEvent);
            uncommitedDomainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Applies a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void ApplyDomainEvent(IDomainEvent domainEvent)
        {
            var type = domainEvent.GetType();
            var stateTransition = stateTransitions[type];
            stateTransition(domainEvent);
        }
    }
}
