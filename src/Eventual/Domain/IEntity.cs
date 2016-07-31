using System;

namespace Eventual.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
