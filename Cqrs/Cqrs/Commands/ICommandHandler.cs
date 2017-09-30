using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
