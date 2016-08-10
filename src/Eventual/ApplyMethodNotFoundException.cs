using System;

namespace Eventual
{
    public class ApplyMethodNotFoundException : Exception
    {
        public Type EventType { get; }

        public ApplyMethodNotFoundException(Type eventType)
        {
            EventType = eventType;
        }

        public override string Message {
            get {
                return $"Could not find Apply method for Event of type {EventType.FullName}";
            }
        }
    }
}