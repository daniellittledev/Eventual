using System.Collections.Generic;
using Eventual.Implementation;

namespace Eventual.TypeDiscovery
{
    public interface IEventApplyContainer
    {
        IReadOnlyCollection<IApplyMethod> ApplyMethods { get; }
    }
}