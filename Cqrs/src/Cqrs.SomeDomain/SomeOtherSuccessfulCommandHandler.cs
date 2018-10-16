using SoderbergPartners.Kalle.Cqrs.Commanding;
using SoderbergPartners.Kalle.Cqrs.Repository;
using System;

namespace SoderbergPartners.Kalle.Cqrs.SomeDomain
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
