using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoderbergPartners.Kalle.Cqrs.Domain;
using SoderbergPartners.Kalle.Cqrs.Repository;
using SoderbergPartners.Kalle.Cqrs.SomeDomain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoderbergPartners.Kalle.Cqrs.Scenarios.Tests
{
    [TestClass]
    public class InMemoryEventStoreTests
    {
        [TestMethod]
        public void Given_AHistoryOfAnAggregate_WhenAnOtherAggregateIsSaved_Then_TheEventsAreStoredInDifferentStreams()
        {
            var oldAggregateId = Guid.NewGuid();
            var history = new HistoryBuilder()
                .WithAggregate(oldAggregateId)
                .WithEvent(new SomeEvent(Guid.Empty))
                .Build();

            var eventStore = new InMemoryEventStore();
            eventStore.SetHistory(history);

            var newEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };
            var newAggregateId = Guid.NewGuid();

            eventStore.SaveDomainEvents(newAggregateId, 0, newEvents);

            var storedEvents = eventStore
                .GetDomainEvents(newAggregateId)
                .ToList();

            var result = DomainEventsVerifier.VerifyEvents(newEvents, storedEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);

            // No changes should have been made to the old stream.
            storedEvents = eventStore
                .GetDomainEvents(oldAggregateId)
                .ToList();

            result = DomainEventsVerifier.VerifyEvents(history[oldAggregateId], storedEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        [ExpectedException(typeof(OptimisticConcurrencyException))]
        public void Given_AHistoryOfTwoEvents_WhenAnotherEventIsSavedSpecifyingAnAggregateVersionOfOne_Then_AnOptimisticConcurrencyExceptionIsThrown()
        {
            var aggregateId = Guid.NewGuid();
            var history = new HistoryBuilder()
                .WithAggregate(aggregateId)
                .WithEvent(new SomeEvent(Guid.Empty))
                .WithEvent(new SomeOtherEvent(Guid.Empty, 1))
                .Build();

            var eventStore = new InMemoryEventStore();
            eventStore.SetHistory(history);

            var newEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };

            eventStore.SaveDomainEvents(aggregateId, 1, newEvents);
        }

        [TestMethod]
        public void Given_AHistoryOfTwoEvents_WhenAnotherEventIsSavedSpecifyingAnAggregateVersionOfTwo_Then_TheTheEventIsStored()
        {
            var aggregateId = Guid.NewGuid();
            var history = new HistoryBuilder()
                .WithAggregate(aggregateId)
                .WithEvent(new SomeEvent(Guid.Empty))
                .WithEvent(new SomeOtherEvent(Guid.Empty, 1))
                .Build();

            var eventStore = new InMemoryEventStore();
            eventStore.SetHistory(history);

            var newEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };

            eventStore.SaveDomainEvents(aggregateId, 2, newEvents);

            var storedEvents = eventStore
                .GetDomainEvents(aggregateId)
                .ToList();

            var expectedResult = history[aggregateId]
                .Concat(newEvents)
                .ToList();

            var result = DomainEventsVerifier.VerifyEvents(expectedResult, storedEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        public void Given_AHistoryWithAnEvent_WhenAnEventIsSaved_Then_TheEventStoreContaintBothEvents()
        {
            var aggregateId = Guid.NewGuid();
            var history = new HistoryBuilder()
                .WithAggregate(aggregateId)
                .WithEvent(new SomeEvent(Guid.Empty))
                .Build();

            var eventStore = new InMemoryEventStore();
            eventStore.SetHistory(history);

            var newEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, 10),
            };

            eventStore.SaveDomainEvents(aggregateId, 1, newEvents);

            var storedEvents = eventStore
                .GetDomainEvents(aggregateId)
                .ToList();

            var expectedResult = history[aggregateId]
                .Concat(newEvents)
                .ToList();

            var result = DomainEventsVerifier.VerifyEvents(expectedResult, storedEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        public void Given_NoHistory_WhenAnEventIsSaved_Then_TheEventStoreContainsTheEvent()
        {
            var eventStore = new InMemoryEventStore();

            var someAggregateId = Guid.NewGuid();
            var someEventId = Guid.NewGuid();
            var newEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(someEventId, 10),
            };

            eventStore.SaveDomainEvents(someAggregateId, 0, newEvents);

            var storedEvents = eventStore
                .GetDomainEvents(someAggregateId)
                .ToList();

            var result = DomainEventsVerifier.VerifyEvents(newEvents, storedEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }
    }
}
