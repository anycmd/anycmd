
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionUpdatedEvent : DomainEvent
    {
        public ElementActionUpdatedEvent(IAcSession acSession, ElementAction source)
            : base(acSession, source)
        {
            this.IsAllowed = source.IsAllowed;
            this.IsAudit = source.IsAudit;
        }

        public string IsAllowed { get; private set; }
        public string IsAudit { get; private set; }
    }
}
