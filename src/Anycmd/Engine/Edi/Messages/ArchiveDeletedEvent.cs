
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ArchiveDeletedEvent : DomainEvent
    {
        public ArchiveDeletedEvent(IAcSession acSession, ArchiveBase source) : base(acSession, source) { }
    }
}
