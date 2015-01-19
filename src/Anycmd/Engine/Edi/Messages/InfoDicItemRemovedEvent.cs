
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemRemovedEvent : DomainEvent
    {
        public InfoDicItemRemovedEvent(IUserSession userSession, InfoDicItemBase source) : base(userSession, source) { }
    }
}
