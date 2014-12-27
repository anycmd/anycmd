
namespace Anycmd.Engine.Ac.Messages.Infra
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