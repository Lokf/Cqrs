using Lokf.Cqrs.Commanding;
using Lokf.Cqrs.Repository;
using System;

namespace Lokf.Cqrs.SomeDomain
{
    public class SomeOtherSuccessfulCommandHandler : ICommandHandler<SomeOtherCommand>
    {
        private readonly IDomainRepository domainRepository;

        public SomeOtherSuccessfulCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public void Handle(SomeOtherCommand command)
        {
            var aggregate = domainRepository.GetAggregate<SomeAggregate>(command.SomeAggregateId);

            aggregate.RaiseSomeOtherEvent(Guid.Empty, command.Value);
        }
    }
}
