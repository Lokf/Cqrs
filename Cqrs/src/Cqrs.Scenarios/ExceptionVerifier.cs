using System;

namespace Lokf.Cqrs.Scenarios
{
    /// <summary>
    /// Verifier of exceptions.
    /// </summary>
    internal class ExceptionVerifier
    {
        /// <summary>
        /// Verifies the actual exception with the expected exception.
        /// </summary>
        /// <remarks>
        /// Only the exception types are verified.
        /// </remarks>
        /// <param name="expected">The expected exception.</param>
        /// <param name="actual">The actual exception.</param>
        /// <returns>The verification result.</returns>
        public static VerificationResult VerifyException(Exception expected, Exception actual)
        {
            if (expected == null && actual == null)
            {
                return VerificationResult.Correct;
            }

            if (expected == null)
            {
                var message = $"Did not expect any exception, but an exception of type {actual.GetType()} was thrown. Message: {actual.Message}.";
                return VerificationResult.Incorrect(message);
            }

            if (actual == null)
            {
                var message = $"An exception of type {expected.GetType()} was expected but no exception was thrown.";
                return VerificationResult.Incorrect(message);
            }

            var expectedType = expected.GetType();
            var actualType = actual.GetType();
            if (expectedType != actualType)
            {
                var message = $"Expected an exception of type {expected.GetType()} but an exception of type {actual.GetType()} was thrown.";
                return VerificationResult.Incorrect(message);
            }

            return VerificationResult.Correct;
        }
    }
}
