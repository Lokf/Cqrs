using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lokf.Cqrs.Domain;
using Lokf.Cqrs.SomeDomain;
using System;
using System.Collections.Generic;

namespace Lokf.Cqrs.Scenarios.Tests
{
    [TestClass]
    public class DomainEventsVerifierTests
    {
        [TestMethod]
        public void Given_ACollectionSomeEvent_When_ItIsVerifiedAgainstTheSameEvent_Then_TheVerificationSucceds()
        {
            var someValue = 10;
            var oneEvent = new List<IDomainEvent>
            {
                new SomeOtherEvent(Guid.Empty, someValue),
            };
            var alsoOneEvent = new IDomainEvent[]
            {
                new SomeOtherEvent(Guid.Empty, someValue),
            };

            var result = DomainEventsVerifier.VerifyEvents(oneEvent, alsoOneEvent);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        public void Given_ACollectionsWithAnEvent_When_ItIsVerifiedAgainstTheSameEventWithADifferentEventId_Then_TheVerificationSucceds()
        {
            var oneEvent = new List<IDomainEvent>
            {
                new SomeEvent(Guid.NewGuid()),
            };
            var alsoOneEvent = new IDomainEvent[]
            {
                new SomeEvent(Guid.NewGuid()),
            };

            var result = DomainEventsVerifier.VerifyEvents(oneEvent, alsoOneEvent);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        public void Given_ACollectionWithTwoEvents_When_ItIsVerifiedAgainstTheSameEventsInADifferentOrder_Then_TheVerificationFails()
        {
            var someEventId = Guid.NewGuid();
            var someValue = 10;
            var anotherSomeEventId = Guid.NewGuid();
            var anotherSomeValue = 20;

            var twoEvents = new List<IDomainEvent>
            {
                new SomeOtherEvent(someEventId, someValue),
                new SomeOtherEvent(anotherSomeEventId, anotherSomeValue),
            };
            var alsoTwoEvents = new IDomainEvent[]
            {
                new SomeOtherEvent(anotherSomeEventId, anotherSomeValue),
                new SomeOtherEvent(someEventId, someValue),
            };

            var result = DomainEventsVerifier.VerifyEvents(twoEvents, alsoTwoEvents);
            Assert.IsFalse(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        public void Given_AnEmptyEventCollection_When_ItIsVerfiedAgainstACollectionWithOneEvent_Then_TheVerificationFails()
        {
            var oneEvent = new List<IDomainEvent>
            {
                new SomeEvent(Guid.Empty),
            };
            var zeroEvents = Array.Empty<IDomainEvent>();

            var result = DomainEventsVerifier.VerifyEvents(oneEvent, zeroEvents);
            Assert.IsFalse(result.IsCorrect, result.Reason);
        }

        [TestMethod]
        public void Given_AnEmptyEventCollection_When_ItIsVerifiedAgainstAnEmptyEventCollection_Then_TheVerificationSucceds()
        {
            var noEvents = new List<IDomainEvent>();
            var zeroEvents = Array.Empty<IDomainEvent>();

            var result = DomainEventsVerifier.VerifyEvents(noEvents, zeroEvents);
            Assert.IsTrue(result.IsCorrect, result.Reason);
        }
    }
}
