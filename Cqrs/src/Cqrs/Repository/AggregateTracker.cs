using Lokf.Cqrs.Domain;
using System.Collections.Generic;

namespace Lokf.Cqrs.Repository
{
    /// <summary>
    /// An aggregate tracker.
    /// </summary>
    public class AggregateTracker
    {
        private readonly HashSet<AggregateRoot> trackedAggregates = new HashSet<AggregateRoot>();

        /// <summary>
        /// Gets the currently tracked aggregates.
        /// </summary>
        public IEnumerable<AggregateRoot> TrackedAggregates => new List<AggregateRoot>(trackedAggregates);

        /// <summary>
        /// Resets the collection of tracked aggregates.
        /// </summary>
        public void ClearTrackedAggregates()
        {
            trackedAggregates.Clear();
        }

        /// <summary>
        /// Tracks the aggregate.
        /// </summary>
        /// <param name="aggregate">The aggreagte.</param>
        public void Track(AggregateRoot aggregate)
        {
            trackedAggregates.Add(aggregate);
        }
    }
}
