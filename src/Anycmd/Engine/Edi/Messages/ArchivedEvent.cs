
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ArchivedEvent : DomainEvent
    {
        public ArchivedEvent(IAcSession acSession, ArchiveBase source)
            : base(acSession, source)
        {
        }
    }
}
