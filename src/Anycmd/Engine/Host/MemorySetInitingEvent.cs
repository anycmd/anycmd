
namespace Anycmd.Engine.Host
{
    using Events;

    public class MemorySetInitingEvent : DomainEvent
    {
        public MemorySetInitingEvent(IMemorySet source)
            : base(source)
        {

        }
    }
}
