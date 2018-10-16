using SoderbergPartners.Kalle.Cqrs.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SoderbergPartners.Kalle.Cqrs.Scenarios
{
    /// <summary>
    /// Verifier of domain events.
    /// </summary>
    public static class DomainEventsVerifier
    {
        private static DomainEventPropertyInfosCache domainEventPropertyInfosCache = new DomainEventPropertyInfosCache();

        /// <summary>
        /// Verifies that the actual events are what is expected.
        /// </summary>
        /// <param name="expected">The expected domain events.</param>
        /// <param name="actual">The actual domain events.</param>
        /// <returns>The verification result.</returns>
        public static VerificationResult VerifyEvents(IReadOnlyCollection<IDomainEvent> expected, IReadOnlyCollection<IDomainEvent> actual)
        {
            if (expected == null && actual == null)
            {
                return VerificationResult.Correct;
            }

            if (expected.Count != actual.Count)
            {
                var message = $"Expected {expected.Count} {(expected.Count == 1 ? "event" : "events")} but {actual.Count} {(expected.Count == 1 ? "event" : "events")} occured.";
                return VerificationResult.Incorrect(message);
            }

            var eventPairs = expected.Zip(actual, (e, a) => new
            {
                Expected = e,
                Actual = a,
            });

            foreach (var eventPair in eventPairs)
            {
                var result = VerifyEvent(eventPair.Expected, eventPair.Actual);
                if (!result.IsCorrect)
                {
                    return result;
                }
            }

            return VerificationResult.Correct;
        }

        private static VerificationResult VerifyEvent(IDomainEvent expected, IDomainEvent actual)
        {
            var expectedType = expected.GetType();
            var actualType = actual.GetType();

            if (expectedType != actualType)
            {
                var message = $"Expected an event of type {expectedType} but the event was of type {actualType}.";
                return VerificationResult.Incorrect(message);
            }

            var propertyInfos = domainEventPropertyInfosCache.GetPropertyInfos(expectedType);
            foreach (var propertyInfo in propertyInfos)
            {
                var expectedValue = expectedType.GetProperty(propertyInfo.Name).GetValue(expected);
                var actualValue = actualType.GetProperty(propertyInfo.Name).GetValue(actual);

                if (!expectedValue.Equals(actualValue))
                {
                    var message = $"Expected {expectedType.Name}.{propertyInfo.Name} to be {expectedValue} but was {actualValue}.";
                    return VerificationResult.Incorrect(message);
                }
            }

            return VerificationResult.Correct;
        }
    }
}
