
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicRemovedEvent : DomainEvent
    {
        public InfoDicRemovedEvent(InfoDicBase source) : base(source) { }
    }
}
