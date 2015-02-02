
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemRemovedEvent : DomainEvent
    {
        public InfoDicItemRemovedEvent(IAcSession userSession, InfoDicItemBase source) : base(userSession, source) { }
    }
}
