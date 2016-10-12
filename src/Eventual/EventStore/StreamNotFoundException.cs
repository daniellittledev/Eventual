using System;

namespace Eventual.EventStore
{
    public class StreamNotFoundException : Exception
    {
        public Guid StreamId { get; }

        public StreamNotFoundException(Guid id)
        {
            StreamId = id;
        }

        public override string Message {
            get {
                return $"Could not find a stream with the Id {StreamId}";
            }
        }
    }
}