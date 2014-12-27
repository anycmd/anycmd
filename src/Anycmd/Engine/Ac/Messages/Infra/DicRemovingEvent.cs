
namespace Anycmd.Engine.Ac.Messages.Infra
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