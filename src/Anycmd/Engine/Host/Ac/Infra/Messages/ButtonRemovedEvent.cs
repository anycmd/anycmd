
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonRemovedEvent : DomainEvent
    {
        public ButtonRemovedEvent(ButtonBase source)
            : base(source)
        {
        }
    }
}