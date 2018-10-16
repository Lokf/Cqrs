using SoderbergPartners.Kalle.Cqrs.Commanding;
using System;

namespace SoderbergPartners.Kalle.Cqrs.SomeDomain
{
    public class SomeOtherCommand : ICommand
    {
        public SomeOtherCommand(Guid someAggregateId, int value)
        {
            SomeAggregateId = someAggregateId;
            Value = value;
        }

        public Guid SomeAggregateId { get; }

        public int Value { get; }
    }
}
