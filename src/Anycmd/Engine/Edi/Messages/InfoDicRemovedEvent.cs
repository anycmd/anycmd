
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicRemovedEvent : DomainEvent
    {
        public InfoDicRemovedEvent(InfoDicBase source) : base(source) { }
    }
}
