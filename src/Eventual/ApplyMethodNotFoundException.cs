using System;

namespace Eventual
{
    public class ApplyMethodNotFoundException : Exception
    {
        public Type EventType { get; set; }

        public ApplyMethodNotFoundException(Type eventType)
            : base($"Could not find Apply method for Event of type {eventType.FullName}")
        {
            EventType = eventType;
        }

    }
}