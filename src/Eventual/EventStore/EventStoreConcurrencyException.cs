using Eventual.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventual.EventStore
{
    public class EventStoreConcurrencyException : Exception
    {
        public Guid StreamId { get; }
        public int CurrentSteamSequence { get; }
        public int LoadedStreamSequence { get; }
        public int Difference { get; }
        public IReadOnlyCollection<string> ExtraEventTypes { get; }

        public EventStoreConcurrencyException(Guid streamId, int loadedStreamSequence, int currentSteamSequence)
        {
            StreamId = streamId;
            LoadedStreamSequence = loadedStreamSequence;
            CurrentSteamSequence = currentSteamSequence;
            Difference = CurrentSteamSequence - LoadedStreamSequence;
        }

        public EventStoreConcurrencyException(Guid streamId, int loadedStreamSequence, int currentSteamSequence, IReadOnlyCollection<object> extraEvents)
            : this (streamId, loadedStreamSequence, currentSteamSequence)
        {
            ExtraEventTypes = extraEvents.Select(x => x.GetType().FullName).ToArray();
        }

        public override string Message {
            get {
                var extraEvents = " extra event(s)";
                var missingEvents = " missing event(s)";
                var differencePhrase = Difference > 0 ? extraEvents : missingEvents;

                var message = $@"Concurrency exception, stream being saved to has been modified.
StreamId: { StreamId}
Loaded Stream Sequence: {LoadedStreamSequence}
Current Stream Sequence: {CurrentSteamSequence}
Difference: {Difference} {differencePhrase}

When updating a stream all events in that stream are first read, in this case there were {LoadedStreamSequence} _
events in the stream when the stream was loaded. Once the events are loaded work is done using them to _
calculate subsequent events. These new events are then appended to the stream. However, in this case another agent _
changed the stream by {Difference} events. This means the new events created using the origial set _
of events is out of date. The action that casued these events to be created must retry using the new up-to-date _
event stream.

"
+ ((Difference > 0) ? (@"Extra Events:
"
+ string.Join(Environment.NewLine, ExtraEventTypes)) : "")
+ $@"
Suggestions
 1) Check if you've got lots of changes happening to a single aggregate, if you do you might need to perform these changes in serial.
 2) If this happens occasionally let the user rety or add automated retries, remember you'll have to load the aggregate again and completely retry.
 3) If the extra events do not conflict register them as non conflicting and they'll be appended after the extra evens, beware this is dangerious if you get it wrong.
";
                return message.Replace("_" + Environment.NewLine, "");
            }
        }
    }
}