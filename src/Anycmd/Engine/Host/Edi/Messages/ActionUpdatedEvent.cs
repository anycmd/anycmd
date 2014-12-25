
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public sealed class ActionUpdatedEvent : DomainEvent {
        /// <summary>
        /// 
        /// </summary>
        #region Ctor
        public ActionUpdatedEvent(ActionBase source)
            : base(source) {
            this.Verb = source.Verb;
            this.Name = source.Name;
            this.IsAllowed = source.IsAllowed;
            this.IsAudit = source.IsAudit;
            this.IsPersist = source.IsPersist;
            this.SortCode = source.SortCode;
        }
        #endregion

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
