﻿using Eventual.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventual.EventStore
{
    public class EventStoreConcurrencyException : Exception
    {
        public Guid StreamId;
        public int CurrentSteamSequence;
        public int LoadedStreamSequence;
        public IReadOnlyCollection<string> ExtraEventTypes;

        public EventStoreConcurrencyException(Guid streamId, int loadedStreamSequence, int currentSteamSequence, IReadOnlyCollection<IPersistedDomainEvent> extraEvents)
        {
            StreamId = streamId;
            LoadedStreamSequence = loadedStreamSequence;
            CurrentSteamSequence = currentSteamSequence;
            ExtraEventTypes = extraEvents.Select(x => x.GetType().FullName).ToArray();
        }

        public override string Message {
            get {
                var numberOfExtraEvents = CurrentSteamSequence - LoadedStreamSequence;
                var message = $@"Concurrency exception, stream being saved to has been modified.
StreamId: { StreamId}
Loaded Stream Sequence: {LoadedStreamSequence}
Current Stream Sequence: {CurrentSteamSequence}

When updating a stream all events in that stream are first read, in this case there were {LoadedStreamSequence} _
events in the stream when the stream was loaded. Once the events are loaded work is done using them to _
calculate subsequent events. These new events are then appended to the stream. However, in this case another agent _
appended {numberOfExtraEvents} events to the same stream. This means the new events created using the origial set _
of events is out of date. The action that casued these events to be created must retry using the new up-to-date _
event stream.

There were {numberOfExtraEvents} extra events in the stream

Extra Events:
"
+ string.Join(Environment.NewLine, ExtraEventTypes)
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