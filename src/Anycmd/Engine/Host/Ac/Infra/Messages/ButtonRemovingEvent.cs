
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class ButtonRemovingEvent: DomainEvent
    {
        public ButtonRemovingEvent(ButtonBase source)
            : base(source)
        {
        }
    }
}