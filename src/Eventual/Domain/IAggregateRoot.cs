namespace Eventual.Domain
{
    public interface IAggregateRoot : IEntity
    {
        int LoadedSequence { get; }
    }
}
