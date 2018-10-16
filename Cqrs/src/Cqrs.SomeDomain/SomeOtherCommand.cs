using Lokf.Cqrs.Commanding;
using System;

namespace Lokf.Cqrs.SomeDomain
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
