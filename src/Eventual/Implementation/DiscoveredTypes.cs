using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eventual.Implementation
{
    public class DiscoveredTypes
    {
        public IReadOnlyCollection<Type> PersistedDomainEventTypes { get; set; }
        public IReadOnlyCollection<IApplyMethod> ApplyExtensionMethods { get; set; }
    }
}