using Lokf.Utils.Extensions.Linq;
using System;
using System.Collections.Generic;

namespace Lokf.Cqrs.Aggregates
{
    /// <summary>
    /// Base class for all aggregate roots. Provides functionallity for raising domain event
    /// and rehydrating the aggregate from its history.
    /// </summary>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// The state changers brings the aggregate from one state to an other when a domain event occurs.
        /// </summary>
        private readonly Dictionary<Type, Action<IDomainEvent>> stateChangers = new Dictionary<Type, Action<IDomainEvent>>();

        private readonly List<IDomainEvent> uncommitedDominEvents = new List<IDomainEvent>();

        /// <summary>
        /// Gets the aggregate ID.
        /// </summary>
        public Guid AggregateId { get; }

        /// <summary>
        /// Initiliazes a new instance of the <cref="AggregateRoot"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        public AggregateRoot(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        /// <summary>
        /// Gets the uncommitted domain events.
        /// </summary>
        public IEnumerable<IDomainEvent> UncommittedDomainEvents => uncommitedDominEvents;

        /// <summary>
        /// Gets the current version of the aggregate, i.e. the number of events that has occurred.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Clears the collection of uncommitted domain events.
        /// </summary>
        public void ClearUncommittedDomainEvents()
        {
            uncommitedDominEvents.Clear();
        }

        /// <summary>
        /// Adds a state changer for the domain event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event.</typeparam>
        /// <param name="stateChanger">The state changer.</param>
        public void AddStateChanger<TDomainEvent>(Action<TDomainEvent> stateChanger)
            where TDomainEvent : IDomainEvent
        {
            stateChangers.Add(typeof(TDomainEvent), domainEvent => stateChanger((TDomainEvent)domainEvent));
        }

        /// <summary>
        /// Brings the aggregate inte its current state by replaying the history.
        /// </summary>
        /// <param name="history">The history of past domain events.</param>
        public void LoadFromHistory(IEnumerable<IDomainEvent> history)
        {
            history.ForEach(ApplyDomainEvent);
        }

        /// <summary>
        /// Raises a new domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            ApplyDomainEvent(domainEvent);
            uncommitedDominEvents.Add(domainEvent);            
        }

        /// <summary>
        /// Applies a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void ApplyDomainEvent(IDomainEvent domainEvent)
        {
            var type = domainEvent.GetType();
            var stateChanger = stateChangers[type];
            stateChanger(domainEvent);
        }
    }
}
