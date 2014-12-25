
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupUpdatedEvent : DomainEvent
    {
        public InfoGroupUpdatedEvent(InfoGroupBase source)
            : base(source)
        {
        }
    }
}
