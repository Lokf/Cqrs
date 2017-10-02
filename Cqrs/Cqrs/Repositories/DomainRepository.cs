using System;
using System.Collections.Generic;
using System.Text;
using Lokf.Cqrs.Aggregates;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Lokf.Cqrs.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private static readonly ConcurrentDictionary<Type, Func<Guid, object>> AggregateConstructors = new ConcurrentDictionary<Type, Func<Guid, object>>();

        private readonly IEventStore eventStore;

        public DomainRepository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public void Add<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }

        public TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : AggregateRoot
        {
            var constructor = AggregateConstructors.GetOrAdd(typeof(TAggregate), GetConstructorFunc<TAggregate>());
            var aggregate = (TAggregate)constructor(aggregateId);

            var history = eventStore.GetDomainEvents(aggregateId);
            aggregate.LoadFromHistory(history);

            return aggregate;
        }

        private Func<Guid, TAggregate> GetConstructorFunc<TAggregate>()
        {
            var parameter = Expression.Parameter(typeof(Guid), "aggregateId");
            var constructor = typeof(TAggregate).GetConstructor(new[] { typeof(Guid) });
            var lambda = Expression.Lambda<Func<Guid, TAggregate>>(Expression.New(constructor, parameter), parameter);
            return lambda.Compile();
        }
    }
}
