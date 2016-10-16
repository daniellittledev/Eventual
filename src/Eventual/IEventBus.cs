using System.Threading.Tasks;

namespace Eventual
{
    public interface IEventBus
    {
        Task PublishAsync(object domainEvent);
    }
}