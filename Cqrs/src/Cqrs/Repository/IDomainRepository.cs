using SoderbergPartners.Kalle.Cqrs.Domain;
using System;

namespace SoderbergPartners.Kalle.Cqrs.Repository
{
    /// <summary>
    /// Defines a repository for accessing and storing aggregates.
    /// </summary>
    public interface IDomainRepository
    {
        /// <summary>
        /// Adds an aggregate to the repository.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregate">The aggregate.</param>
        void Add<TAggregate>(TAggregate aggregate)
            where TAggregate : AggregateRoot;

        /// <summary>
        /// Gets the aggregate with the given ID.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregateId">The ID of the aggregate.</param>
        /// <returns>Returns the aggregate with the ID</returns>
        TAggregate GetAggregate<TAggregate>(Guid aggregateId)
            where TAggregate : AggregateRoot;
    }
}
