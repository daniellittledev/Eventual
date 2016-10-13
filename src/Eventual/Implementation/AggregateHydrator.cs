using System;
using Eventual.Domain;
using Eventual.EventStore;
using System.Reflection;

namespace Eventual.Implementation
{
    public class AggregateHydrator<T> : IAggregateHydrator<T>
        where T : class, IAggregateRoot
    {
        private readonly IEventApplicator eventApplicator;

        public AggregateHydrator(IEventApplicator eventApplicator)
        {
            this.eventApplicator = eventApplicator;
        }

        public T Hydrate(AggregateStream stream)
        {
            var argTypes = new Type[] { typeof(Guid), typeof(int) };
            var args = new object[] { stream.StreamId, stream.LatestSequence };
            var aggregate = Construct<T>(argTypes, args);

            foreach (var @event in stream.Events) {
                aggregate = eventApplicator.ApplyEvent(aggregate, @event);
            }

            return aggregate;
        }

        public static TType Construct<TType>(Type[] paramTypes, object[] paramValues)
        {
            Type t = typeof(TType);

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var constructorInfos = t.GetConstructors(flags);

            foreach (var constructorInfo in constructorInfos) {
                var parameters = constructorInfo.GetParameters();

                if (paramTypes.Length != parameters.Length) {
                    continue;
                }

                var thisIsIt = true;
                var length = parameters.Length;
                for (int i = 0; i < length; i++) {
                    if (paramTypes[i] != parameters[i].ParameterType) {
                        thisIsIt = false;
                        break;
                    }
                }

                if (thisIsIt) {
                    return (TType)constructorInfo.Invoke(paramValues);
                }
            }

            throw new ConstructorMissingException(paramTypes);
        }
    }
}