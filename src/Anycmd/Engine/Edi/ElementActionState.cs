using System;

namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using Host.Ac;
    using Model;
    using Util;

    public sealed class ElementActionState : StateObject<ElementActionState>, IElementAction, IStateObject
    {
        private string _isAudit;
        private string _isAllowed;

        private ElementActionState(Guid id) : base(id) { }

        internal static ElementActionState Create(IElementAction elementAction)
        {
            if (elementAction == null)
            {
                throw new ArgumentNullException("elementAction");
            }
            var data = new ElementActionState(elementAction.Id)
            {
                ActionId = elementAction.ActionId,
                ElementId = elementAction.ElementId,
                IsAllowed = elementAction.IsAllowed,
                IsAudit = elementAction.IsAudit
            };

            return data;
        }

        // 不要在这里验证ActionID的合法性
        public Guid ActionId { get; private set; }

        // 不要在这里验证ElementID的合法性
        public Guid ElementId { get; private set; }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        public string IsAudit
        {
            get { return _isAudit; }
            private set
            {
                if (value != _isAudit)
                {
                    _isAudit = value;
                    AuditType auditType;
                    if (!value.TryParse(out auditType))
                    {
                        throw new AnycmdException("意外的AuditType:" + value);
                    }
                    this.AuditType = auditType;
                }
            }
        }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        public AuditType AuditType { get; private set; }

        /// <summary>
        /// 是否允许
        /// </summary>
        public string IsAllowed
        {
            get { return _isAllowed; }
            private set
            {
                if (value != _isAllowed)
                {
                    _isAllowed = value;
                    AllowType allowType;
                    if (!value.TryParse(out allowType))
                    {
                        throw new AnycmdException("意外的AllowType:" + value);
                    }
                    this.AllowType = allowType;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AllowType AllowType { get; private set; }

        protected override bool DoEquals(ElementActionState other)
        {
            return
                Id == other.Id &&
                ElementId == other.ElementId &&
                ActionId == other.ActionId &&
                AuditType == other.AuditType &&
                AllowType == other.AllowType;
        }
    }
}
