
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicRemovedEvent : DomainEvent
    {
        public InfoDicRemovedEvent(IAcSession userSession, InfoDicBase source) : base(userSession, source) { }
    }
}
