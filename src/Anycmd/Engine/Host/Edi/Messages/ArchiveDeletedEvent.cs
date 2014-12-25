
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ArchiveDeletedEvent : DomainEvent
    {
        public ArchiveDeletedEvent(ArchiveBase source) : base(source) { }
    }
}
