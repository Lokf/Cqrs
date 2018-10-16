namespace SoderbergPartners.Kalle.Cqrs.Commanding
{
    /// <summary>
    /// A command handler of type <see cref="ICommandHandler{TCommand}"/> knows how to handle
    /// a command of type <typeparamref name="TCommand"/>.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(TCommand command);
    }
}
