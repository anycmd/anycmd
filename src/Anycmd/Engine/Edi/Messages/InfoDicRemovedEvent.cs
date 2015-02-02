
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicRemovedEvent : DomainEvent
    {
        public InfoDicRemovedEvent(IAcSession acSession, InfoDicBase source) : base(acSession, source) { }
    }
}
