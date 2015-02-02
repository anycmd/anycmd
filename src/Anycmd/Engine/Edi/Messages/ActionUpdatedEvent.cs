
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class ActionUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public ActionUpdatedEvent(IAcSession acSession, ActionBase source)
            : base(acSession, source)
        {
            this.Verb = source.Verb;
            this.Name = source.Name;
            this.IsAllowed = source.IsAllowed;
            this.IsAudit = source.IsAudit;
            this.IsPersist = source.IsPersist;
            this.SortCode = source.SortCode;
        }

        public string Verb { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string IsAllowed { get; private set; }
        public string IsAudit { get; private set; }
        public bool IsPersist { get; private set; }
        public int SortCode { get; private set; }
    }
}
