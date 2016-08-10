using System;
using System.Collections.Generic;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class ValidationMessage
    {
        public string Message { get; set; }
        public string Location { get; set; }

        public ValidationMessage(string message)
        {
            Message = message;
        }

        public ValidationMessage(string location, string message)
        {
            Message = message;
            Location = location;
        }
    }

    public class ValidationFailureEvent : IDomainEvent
    {
        public IReadOnlyCollection<ValidationMessage> Reasons { get; }

        public ValidationFailureEvent(IReadOnlyCollection<ValidationMessage> reasons)
        {
            this.Reasons = reasons;
        }
    }

    public class ValidationFailureList
    {
        private List<ValidationMessage> Reasons { get; }

        public ValidationFailureList()
        {
            Reasons = new List<ValidationMessage>();
        }

        public ValidationFailureList Add(string location, string message)
        {
            Reasons.Add(new ValidationMessage(location, message));

            return this;
        }

        public ValidationFailureEvent AsEvent()
        {
            return new ValidationFailureEvent(Reasons);
        }

        public bool HasFailures => (Reasons.Count > 0);
    }
}
