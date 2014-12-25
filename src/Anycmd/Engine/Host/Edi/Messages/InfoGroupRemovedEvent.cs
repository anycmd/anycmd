
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupRemovedEvent : DomainEvent
    {
        public InfoGroupRemovedEvent(InfoGroupBase source) : base(source) { }
    }
}
