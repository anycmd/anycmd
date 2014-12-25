
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemRemovedEvent : DomainEvent
    {
        public InfoDicItemRemovedEvent(InfoDicItemBase source) : base(source) { }
    }
}
