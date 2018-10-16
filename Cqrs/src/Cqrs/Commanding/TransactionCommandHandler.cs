using Lokf.Cqrs.Repository;

namespace Lokf.Cqrs.Commanding
{
    /// <summary>
    /// Wraps an inner command handler in a transaction. The unit of work performed inside the inner command handler is
    /// committed and stored as a whole. If something fails, then the unit of work is rollbacked.
    /// </summary>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    public class TransactionCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> innerCommandHandler;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionCommandHandler{TCommand}"/> class.
        /// </summary>
        /// <param name="innerCommandHandler">The inner command handler.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public TransactionCommandHandler(ICommandHandler<TCommand> innerCommandHandler, IUnitOfWork unitOfWork)
        {
            this.innerCommandHandler = innerCommandHandler;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles the command within a transaction.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Handle(TCommand command)
        {
            try
            {
                innerCommandHandler.Handle(command);
                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
