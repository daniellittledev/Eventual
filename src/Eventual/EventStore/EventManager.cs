using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventual.Concurrency;
using Eventual.EventTypes;

namespace Eventual.EventStore
{
    public class EventManager : IEventManager
    {
        private readonly IEventStore eventStore;
        private readonly IConflictResolver conflictResolver;
        private readonly IEventClassifier eventClassifier;

        public EventManager(IEventStore eventStore, IConflictResolver conflictResolver, IEventClassifier eventClassifier)
        {
            this.eventStore = eventStore;
            this.conflictResolver = conflictResolver;
            this.eventClassifier = eventClassifier;
        }

        public Task<AggregateStream> GetStreamAsync(Guid streamId)
        {
            return eventStore.GetStreamAsync(streamId);
        }

        public async Task SaveAsync(Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents)
        {
            var latestSequence = 0;
            var extraEvents = new List<Type>();

            var result = await RetryIf(5, async () =>
            {
                var newEvent = extraEvents.Count;
                var attempt = await eventStore.TryAppendToStreamAsync(streamId, loadedSequence + newEvent, domainEvents);

                if (attempt.Result == Result.Success) return ShouldRetry.No;


                var newEvents = (await eventStore.GetEventsAfterSequenceAsync(streamId, loadedSequence + newEvent)).ToArray();
                var newEventTypes = newEvents.Select(x => eventClassifier.GetTypeForAliases(x.TypeName)).ToArray();

                extraEvents.AddRange(newEventTypes);
                latestSequence = newEvents[newEvents.Length - 1].Sequence;

                if (domainEvents.Any(domainEvent => conflictResolver.ConflictsWith(domainEvent.GetType(), newEventTypes))) {
                    throw new EventStoreConcurrencyException(streamId, loadedSequence, latestSequence, newEventTypes);
                }

                await eventStore.LockStreamAsync(streamId);

                return ShouldRetry.Yes;
            });

            if (result == ExitStatus.TooManyRetries)
            {
                throw new EventStoreConcurrencyException(streamId, loadedSequence, latestSequence, extraEvents);
            }
        }

        private static async Task<ExitStatus> RetryIf(int timesToRetry, Func<Task<ShouldRetry>> func)
        {
            var retriesLeft = timesToRetry;
            while (true)
            {
                if (await func() == ShouldRetry.Yes)
                {
                    retriesLeft -= 1;

                    if (retriesLeft <= 0)
                    {
                        return ExitStatus.TooManyRetries;
                    }

                    await Task.Delay((timesToRetry - retriesLeft)*100);
                }
                else
                {
                    return ExitStatus.Completed;
                }
            }
        }
    }
}