using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual
{
    public interface IEventBus
    {
        Task PublishAsync(IDomainEvent domainEvent);
    }
}