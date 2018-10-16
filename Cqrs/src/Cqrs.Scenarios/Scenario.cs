using SoderbergPartners.Kalle.Cqrs.Commanding;
using SoderbergPartners.Kalle.Cqrs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoderbergPartners.Kalle.Cqrs.Scenarios
{
    /// <summary>
    /// A scenario is e specification of the expected behaviour when a command is executed given a specific state.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    public sealed class Scenario<TCommand>
        where TCommand : ICommand
    {
        private readonly TCommand command;
        private readonly ICommandHandler<TCommand> commandHandler;
        private readonly InMemoryEventStore eventStore;
        private readonly List<IDomainEvent> expectedDomainEvents;
        private readonly Exception expectedException;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario{TCommand}"/> class.
        /// </summary>
        /// <param name="given">The history of domain events.</param>
        /// <param name="when">The command.</param>
        /// <param name="commandHandler">The command handler.</param>
        /// <param name="then">The expected domain events.</param>
        public Scenario(Dictionary<Guid, List<IDomainEvent>> given, TCommand when, ICommandHandler<TCommand> commandHandler, List<IDomainEvent> then)
            : this(given, when, commandHandler, then, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario{TCommand}"/> class.
        /// </summary>
        /// <param name="given">The history of domain events.</param>
        /// <param name="when">The command.</param>
        /// <param name="commandHandler">The command handler.</param>
        /// <param name="then">The expected exception.</param>
        public Scenario(Dictionary<Guid, List<IDomainEvent>> given, TCommand when, ICommandHandler<TCommand> commandHandler, Exception then)
            : this(given, when, commandHandler, null, then) { }

        private Scenario(Dictionary<Guid, List<IDomainEvent>> given, TCommand when, ICommandHandler<TCommand> commandHandler, List<IDomainEvent> expectedDomainEvents, Exception expectedException)
        {
            eventStore = new InMemoryEventStore();
            eventStore.SetHistory(given);
            command = when;
            this.commandHandler = commandHandler;
            this.expectedDomainEvents = expectedDomainEvents;
            this.expectedException = expectedException;
        }

        /// <summary>
        /// Verifies the expected behaviour of the scenario.
        /// </summary>
        /// <returns>The verification result.</returns>
        public VerificationResult Verify()
        {
            Exception actualException = null;
            List<IDomainEvent> actualDomainEvents = null;
            try
            {
                commandHandler.Handle(command);
                actualDomainEvents = eventStore
                    .PeekChanges()
                    .ToList();

                var result = DomainEventsVerifier.VerifyEvents(expectedDomainEvents, actualDomainEvents);
                if (!result.IsCorrect)
                {
                    return result;
                }
            }
            catch (Exception exception)
            {
                actualException = exception;
            }

            return ExceptionVerifier.VerifyException(expectedException, actualException);
        }
    }
}
