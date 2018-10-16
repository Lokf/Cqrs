namespace Lokf.Cqrs.Scenarios
{
    /// <summary>
    /// The result of a verification.
    /// </summary>
    public class VerificationResult
    {
        private VerificationResult(bool isCorrect, string reason = null)
        {
            IsCorrect = isCorrect;
            Reason = reason;
        }

        /// <summary>
        /// Gets a value indicating whether the verfication proved the tested behaviour correct or not.
        /// </summary>
        public bool IsCorrect { get; }

        /// <summary>
        /// Gets the reason why the verificiation was incorrect.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Gets a verfication result indicating a correct behaviour..
        /// </summary>
        internal static VerificationResult Correct => new VerificationResult(true);

        /// <summary>
        /// Get a verification result indicating a incorrect behaviour.
        /// </summary>
        /// <param name="reason">The reason for why the verification was incorrect.</param>
        /// <returns>The verification result indicating the incorrect behaviour.</returns>
        internal static VerificationResult Incorrect(string reason) => new VerificationResult(false, reason);
    }
}
