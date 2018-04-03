using Lokf.Cqrs.Aggregates;
using Lokf.Cqrs.Commands;
using Lokf.Cqrs.EventStores;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lokf.Cqrs.CqrsTestFramework
{
    public sealed class Scenario2<TCommand>
        where TCommand : ICommand
    {
        private readonly IEnumerable<IDomainEvent> given;
        private readonly TCommand when;
        private readonly ICommandHandler<ICommand> commandHandler;
        private readonly List<IDomainEvent> expectedEvents;
        private readonly Exception expectedException;

        private Scenario2(IEnumerable<IDomainEvent> given, TCommand when, ICommandHandler<ICommand> commandHandler, IEnumerable<IDomainEvent> expectedEvents, Exception expectedException)
        {
            this.given = given;
            this.when = when;
            this.commandHandler = commandHandler;
            this.expectedEvents = expectedEvents.ToList();
            this.expectedException = expectedException;
        }

        public Scenario2(IEnumerable<IDomainEvent> given, TCommand when, ICommandHandler<ICommand> commandHandler, IEnumerable<IDomainEvent> then)
            : this(given, when, commandHandler, then, null)
        {
        }

        public Scenario2(IEnumerable<IDomainEvent> given, TCommand when, ICommandHandler<ICommand> commandHandler, Exception then)
            : this(given, when, commandHandler, null, then)
        {
        }
                
        public void Verify()
        {
            var eventStore = new InMemoryEventStore();
            eventStore.SetHistory(given);

            Exception caughtException = null;

            try
            {
                commandHandler.Execute(when);

                var actual = eventStore
                    .PeekChanges()
                    .ToList();

                CompareEvents(expectedEvents, actual);
            }
            catch (AssertionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                caughtException = exception;
            }
            finally
            {
                CompareExceptions(expectedException, caughtException);
            }
        }

        private void CompareEvent(IDomainEvent expected, IDomainEvent actual)
        {
            var expectedType = expected.GetType();
            var actualType = actual.GetType();

            var message = $"Expected an event of type { expectedType } but the event was of type { actualType }.";
            Assert.AreEqual(expectedType, actualType, message);

            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            foreach (var propertyInfo in expectedType.GetProperties(bindingFlags))
            {
                if (propertyInfo.Name == "Id")
                {
                    // The ID of the events does not have to be the same.
                    continue;
                }

                var expectedValue = expectedType.GetProperty(propertyInfo.Name).GetValue(expected);
                var actualValue = actualType.GetProperty(propertyInfo.Name).GetValue(actual);

                message = $"Expected { expectedType.Name }.{ propertyInfo.Name } to be { expectedValue } but was { actualValue }";
                Assert.AreEqual(expectedValue, actualValue, message);
            }
        }

        private void CompareEvents(IReadOnlyCollection<IDomainEvent> expected, IReadOnlyCollection<IDomainEvent> actual)
        {
            var message = $"Expected {expected.Count} {(expected.Count == 1 ? "event" : "events")} but {actual.Count} {(expected.Count == 1 ? "event" : "events")} occured.";

            Assert.AreEqual(expected.Count, actual.Count, message);

            var eventPairs = expected.Zip(actual, (e, a) => new { Expected = e, Actual = a });
            foreach (var eventPair in eventPairs)
            {
                CompareEvent(eventPair.Expected, eventPair.Actual);
            }
        }

        private void CompareExceptions(Exception expected, Exception actual)
        {
            string message;

            if (expected == null)
            {
                if (actual == null)
                {
                    return;
                }

                message = $"Did not expect any exception, but an exception of type {actual.GetType()} was thrown.";

                Assert.Fail(message);
            }

            if (actual == null)
            {
                message = $"An exception of type {expected.GetType()} was expected but no exception was thrown.";

                Assert.Fail(message);
            }

            message = $"Expected an exception of type {expected.GetType()} but an exception of type {actual.GetType()} was thrown";

            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}
