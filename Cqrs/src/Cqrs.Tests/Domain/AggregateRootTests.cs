using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoderbergPartners.Kalle.Cqrs.Domain;
using SoderbergPartners.Kalle.Cqrs.SomeDomain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoderbergPartners.Kalle.Cqrs.Tests.Domain
{
    [TestClass]
    public class AggregateRootTests
    {
        [TestMethod]
        public void Given_AnAggregate_When_AnEventIsRaised_Then_TheEventIsAddedAsUncommitted()
        {
            var eventId = Guid.NewGuid();
            var aggregate = new SomeAggregate(Guid.Empty);
            aggregate.RaiseSomeEvent(eventId);

            var uncommittedEventId = aggregate
                .UncommittedDomainEvents
                .Single()
                .EventId;

            Assert.AreEqual(eventId, uncommittedEventId);
        }

        [TestMethod]
        public void Given_AnAggregateWithAStateTransitionRegistered_When_AnEventIsRaised_Then_TheStateTransitionIsCalled()
        {
            var eventId = Guid.NewGuid();
            var aggregate = new SomeAggregate(Guid.Empty);
            aggregate.RaiseSomeEvent(eventId);

            Assert.AreEqual(eventId, aggregate.EventId);
        }

        [TestMethod]
        public void Given_AnAggregateWithUncommittedEvents_When_TheUncommittedEventsAreCleared_Then_ThereAreZeroUncommittedEventsLeft()
        {
            var aggregate = new SomeAggregate(Guid.Empty);
            aggregate.RaiseSomeEvent(Guid.Empty);

            // Make sure that there are uncommitted events that are cleared to avoid false positive.
            Assert.AreEqual(1, aggregate.UncommittedDomainEvents.Count());

            aggregate.ClearUncommittedDomainEvents();

            Assert.AreEqual(0, aggregate.UncommittedDomainEvents.Count());
        }

        [TestMethod]
        public void Given_AnAggregate_When_ItsHistoryIsLoaded_Then_TheStateIsBroughtBackByPlayingAllStateTransitions()
        {
            var someEventId = Guid.NewGuid();
            var someEvent = new SomeEvent(someEventId);
            var someOtherEvent = new SomeOtherEvent(Guid.Empty, 10);
            var anotherOne = new SomeOtherEvent(Guid.Empty, 20);

            var history = new List<IDomainEvent>
            {
                someEvent,
                someOtherEvent,
                anotherOne,
            };

            var aggregate = new SomeAggregate(Guid.Empty);
            aggregate.LoadFromHistory(history);

            Assert.AreEqual(someEventId, aggregate.EventId);
            Assert.AreEqual(30, aggregate.Value);
            Assert.AreEqual(3, aggregate.Version);
        }

        [TestMethod]
        public void Given_AnAggregate_When_ItsHistoryIsLoaded_Then_TheOrderIsPreserved()
        {
            var someEventId = Guid.NewGuid();
            var someEvent = new SomeEvent(someEventId);
            var andAnotherOne = Guid.NewGuid();
            var andAnotherEvent = new SomeEvent(andAnotherOne);

            var history = new List<IDomainEvent>
            {
                someEvent,
                andAnotherEvent,
            };

            var aggregate = new SomeAggregate(Guid.Empty);
            aggregate.LoadFromHistory(history);

            Assert.AreEqual(andAnotherOne, aggregate.EventId);
        }
    }
}
