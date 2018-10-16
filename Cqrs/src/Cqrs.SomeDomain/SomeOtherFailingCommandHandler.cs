using SoderbergPartners.Kalle.Cqrs.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoderbergPartners.Kalle.Cqrs.SomeDomain
{
    public class SomeOtherFailingCommandHandler : ICommandHandler<SomeOtherCommand>
    {
        public void Handle(SomeOtherCommand command)
        {
            throw new NotSupportedException();
        }
    }
}
