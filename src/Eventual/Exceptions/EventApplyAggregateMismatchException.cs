using System;

namespace Eventual.Implementation
{
    public class EventApplyAggregateMismatchException : Exception
    {
        public Type ApplyAggregateRootType { get; }
        public Type ExpectedAggregateType { get; }
        public Type EventType { get; }

        public EventApplyAggregateMismatchException(Type expectedAggregateType, Type applyAggregateRootType, Type eventType)
        {
            this.ExpectedAggregateType = expectedAggregateType;
            this.ApplyAggregateRootType = applyAggregateRootType;
            this.EventType = eventType;
        }

        public override string Message => $"The Apply function for the event {EventType} expects the aggregate {ApplyAggregateRootType}, however the aggregate being hydrated was of type {ExpectedAggregateType}";
    }
}