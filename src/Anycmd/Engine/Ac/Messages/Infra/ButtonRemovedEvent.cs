
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonRemovedEvent : DomainEvent
    {
        public ButtonRemovedEvent(IAcSession acSession, ButtonBase source)
            : base(acSession, source)
        {
        }
    }
}