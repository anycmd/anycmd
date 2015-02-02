
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemRemovedEvent : DomainEvent
    {
        public InfoDicItemRemovedEvent(IAcSession acSession, InfoDicItemBase source) : base(acSession, source) { }
    }
}
