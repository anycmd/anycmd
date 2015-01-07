
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class ButtonRemovingEvent: DomainEvent
    {
        public ButtonRemovingEvent(ButtonBase source)
            : base(source)
        {
        }
    }
}