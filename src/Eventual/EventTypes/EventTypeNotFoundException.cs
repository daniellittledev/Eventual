using System;

namespace Eventual.EventTypes
{
    public class EventTypeNotFoundException : Exception
    {
        public EventTypeNotFoundException(string eventAlias)
        {
            this.EventAlias = eventAlias;
        }

        public string EventAlias { get; }

        public override string Message => $"Could not find a type that matches the Event alias {EventAlias}";
    }
}