
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonRemovedEvent : DomainEvent
    {
        public UiViewButtonRemovedEvent(IAcSession acSession, UiViewButtonBase source)
            : base(acSession, source)
        {
        }
    }
}
