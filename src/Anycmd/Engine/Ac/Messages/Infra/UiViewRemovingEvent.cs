
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class UiViewRemovingEvent: DomainEvent
    {
        public UiViewRemovingEvent(UiViewBase source)
            : base(source)
        {
        }
    }
}
