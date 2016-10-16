using System;
using System.Collections.Generic;

namespace Eventual.TypeDiscovery
{
    public interface IEventTypeContainer
    {
        IReadOnlyCollection<Type> EventTypes { get; }
    }
}