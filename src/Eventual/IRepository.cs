using System;
using System.Threading.Tasks;
using Eventual.Domain;
using Eventual.MessageContracts;

namespace Eventual
{
    public interface IRepository<T> 
        where T : IAggregateRoot
    {
        Task<T> LoadAsync(Guid aggregateId);
        Task SaveAsync(T aggregate, params IDomainEvent[] domainEvents);
    }
}