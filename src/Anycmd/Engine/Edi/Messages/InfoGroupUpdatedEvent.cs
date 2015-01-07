
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
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
