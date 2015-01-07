
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class DicRemovingEvent: DomainEvent
    {
        public DicRemovingEvent(DicBase source)
            : base(source)
        {
        }
    }
}