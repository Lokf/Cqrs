using SoderbergPartners.Kalle.Cqrs.Domain;
using System;
using System.Collections.Generic;

namespace SoderbergPartners.Kalle.Cqrs.Scenarios
{
    /// <summary>
    /// A builder of a history of domain events.
    /// </summary>
    public class HistoryBuilder
    {
        private readonly Dictionary<Guid, List<IDomainEvent>> history = new Dictionary<Guid, List<IDomainEvent>>();
        private List<IDomainEvent> currentStream;

        /// <summary>
        /// Builds the history.
        /// </summary>
        /// <returns>The history.</returns>
        public Dictionary<Guid, List<IDomainEvent>> Build()
        {
            return history;
        }

        /// <summary>
        /// Starts a new event stream for the given aggregate ID.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>The history builder itself for allowing a fluent API.</returns>
        public HistoryBuilder WithAggregate(Guid aggregateId)
        {
            currentStream = new List<IDomainEvent>();
            history.Add(aggregateId, currentStream);

            return this;
        }

        /// <summary>
        /// Adds an event to the current event stream.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <returns>The history builder itself for allowing a fluent API.</returns>
        public HistoryBuilder WithEvent(IDomainEvent domainEvent)
        {
            currentStream.Add(domainEvent);

            return this;
        }
    }
}
