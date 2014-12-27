
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionUpdatedEvent : DomainEvent
    {
        #region Ctor
        public ElementActionUpdatedEvent(ElementAction source)
            : base(source)
        {
            this.IsAllowed = source.IsAllowed;
            this.IsAudit = source.IsAudit;
        }
        #endregion

        public string IsAllowed { get; private set; }
        public string IsAudit { get; private set; }
    }
}
