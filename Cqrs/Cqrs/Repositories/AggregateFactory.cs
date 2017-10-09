using Lokf.Cqrs.Aggregates;
using System;
using System.Linq.Expressions;

namespace Lokf.Cqrs.Repositories
{
    /// <summary>
    /// A factory for creating new instances of aggregates. The class is static so that the 
    /// compiled lambda for the constructor is cached during the lifetime of the application.
    /// </summary>
    /// <typeparam name="TAggregate">The type of aggregate.</typeparam>
    internal static class AggregateFactory<TAggregate>
        where TAggregate : AggregateRoot
    {
        /// <summary>
        /// Cached constructor delegate.
        /// </summary>
        private static readonly Func<Guid, TAggregate> Constructor;

        /// <summary>
        /// Creates a new instance of the <see cref="TAggregate"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate ID.</param>
        /// <returns>A new instance of the <see cref="TAggregate"/> class.</returns>
        public static TAggregate Create(Guid aggregateId)
        {
            return Constructor(aggregateId);
        }

        /// <summary>
        /// Creates the constructor delegate once and then its cached.
        /// </summary>
        static AggregateFactory()
        {
            var parameter = Expression.Parameter(typeof(Guid), "aggregateId");
            var constructor = typeof(TAggregate).GetConstructor(new[] { typeof(Guid) });
            var lambda = Expression.Lambda<Func<Guid, TAggregate>>(Expression.New(constructor, parameter), parameter);
            Constructor = lambda.Compile();
        }
    }
}
