using System;
using System.Collections.Generic;

namespace Eventual.EventTypes
{
    public interface IEventClassifier
    {
        bool IsPersistedEvent(Type eventType);

        Type GetTypeForAliases(string eventAlias);
    }
}