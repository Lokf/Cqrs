using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lokf.Cqrs.Domain;
using Lokf.Cqrs.SomeDomain;
using System;
using System.Collections.Generic;

namespace Lokf.Cqrs.Scenarios.Tests
{
    [TestClass]
    public class ScenarioTests
    {
        [TestMethod]
        public void Given_AFailingCommandExecution_When_ItIsVerifiedAgainstAnExpectedException_Then_TheVerificationIsCorrect()
        {
            var scenario = new Scenario<SomeOtherCommand>(new Dictionary<Guid, List<IDomainEvent>>(), new SomeOtherCommand(Guid.Empty, 10), new SomeOtherFailingCommandHandler(), new NotSupportedException());
            var result = scenario.Verify();

            Assert.IsTrue(result.IsCorrect);
        }

        [TestMethod]
        public void Given_AFailingCommandExecution_When_ItIsVerifiedAgainstAListOfEvents_Then_TheVerificationIsIncorrect()
        {
            var scenario = new Scenario<SomeOtherCommand>(new Dictionary<Guid, List<IDomainEvent>>(), new SomeOtherCommand(Guid.Empty, 10), new SomeOtherFailingCommandHandler(), new List<IDomainEvent>());
            var result = scenario.Verify();

            Assert.IsFalse(result.IsCorrect);
        }

        [TestMethod]
        public void Given()
        {
            var scenario = new Scenario<SomeOtherCommand>(new Dictionary<Guid, List<IDomainEvent>>(), new SomeOtherCommand(Guid.Empty, 10), new SomeOtherSuccessfulCommandHandler(null), new List<IDomainEvent> { new SomeOtherEvent(Guid.Empty, 10) });
            var result = scenario.Verify();

            Assert.IsFalse(result.IsCorrect);
        }
    }
}
