
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class DicRemovingEvent: DomainEvent
    {
        public DicRemovingEvent(DicBase source)
            : base(source)
        {
        }
    }
}