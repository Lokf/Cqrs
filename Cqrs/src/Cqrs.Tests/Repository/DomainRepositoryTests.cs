using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SoderbergPartners.Kalle.Cqrs.Domain;
using SoderbergPartners.Kalle.Cqrs.Repository;
using SoderbergPartners.Kalle.Cqrs.SomeDomain;
using System;
using System.Collections.Generic;

namespace SoderbergPartners.Kalle.Cqrs.Tests
{
    [TestClass]
    public class DomainRepositoryTests
    {
        [TestMethod]
        public void Given_AnAggregateWithAHistory_When_TheAggregateIsLoaded_Then_TheHistoryIsReplayed()
        {
            var aggregegateId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var someEvent = new SomeEvent(eventId);

            var history = new List<IDomainEvent>
            {
                someEvent,
            };

            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(x => x.GetDomainEvents(aggregegateId))
                .Returns(history);

            var repository = new DomainRepository(eventStore.Object, new AggregateTracker());
            var aggregate = repository.GetAggregate<SomeAggregate>(aggregegateId);

            Assert.AreEqual(eventId, aggregate.EventId);
        }
    }
}
