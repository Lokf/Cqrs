using Lokf.Cqrs.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lokf.Cqrs.SomeDomain
{
    public class SomeOtherFailingCommandHandler : ICommandHandler<SomeOtherCommand>
    {
        public void Handle(SomeOtherCommand command)
        {
            throw new NotSupportedException();
        }
    }
}
