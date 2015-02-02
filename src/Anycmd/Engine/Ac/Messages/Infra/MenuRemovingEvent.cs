
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class MenuRemovingEvent: DomainEvent
    {
        public MenuRemovingEvent(IAcSession acSession, MenuBase source)
            : base(acSession, source)
        {
        }
    }
}