
namespace Anycmd.Engine.Host
{
    using Events;

    public class MemorySetInitializedEvent : DomainEvent
    {
        public MemorySetInitializedEvent(IMemorySet source)
            : base(source)
        {

        }
    }
}
