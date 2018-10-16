namespace Lokf.Cqrs.Repository
{
    /// <summary>
    /// A unit of work is something that should be persisted in full or not at all.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits the unit of work.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks the unit of work.
        /// </summary>
        void Rollback();
    }
}
