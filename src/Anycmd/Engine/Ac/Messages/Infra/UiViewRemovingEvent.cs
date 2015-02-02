
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class UiViewRemovingEvent: DomainEvent
    {
        public UiViewRemovingEvent(IAcSession userSession, UiViewBase source)
            : base(userSession, source)
        {
        }
    }
}
