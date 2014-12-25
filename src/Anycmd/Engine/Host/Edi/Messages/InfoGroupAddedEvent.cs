
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupAddedEvent : DomainEvent
    {
        public InfoGroupAddedEvent(InfoGroupBase source) : base(source) { }
    }
}
