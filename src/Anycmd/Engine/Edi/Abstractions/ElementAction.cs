
namespace Anycmd.Engine.Edi.Abstractions
{
    using Engine.Host.Ac;
    using Exceptions;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 本体元素级动作。<see cref="IElementAction"/>
    /// </summary>
    public sealed class ElementAction : EntityBase, IElementAction
    {
        private Guid _elementId;
        private Guid _actionId;
        private string _isAudit;
        private string _isAllowed;

        public ElementAction() { }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ElementId
        {
            get { return _elementId; }
            set
            {
                if (value == _elementId) return;
                if (_elementId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改所属元素");
                }
                // 不要验证ElementId标识的本体元素的存在性，因为ElementAction与Element是一起存储和读取的。
                _elementId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ActionId
        {
            get { return _actionId; }
            set
            {
                if (value == _actionId) return;
                if (_actionId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改所属动作");
                }
                _actionId = value;
            }
        }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        public string IsAudit
        {
            get { return _isAudit; }
            set
            {
                if (value == _isAudit) return;
                _isAudit = value;
                AuditType auditType;
                if (!value.TryParse(out auditType))
                {
                    throw new AnycmdException("意外的AuditType:" + value);
                }
                this.AuditType = auditType;
            }
        }

        /// <summary>
        /// 是否允许
        /// </summary>
        public string IsAllowed
        {
            get { return _isAllowed; }
            set
            {
                if (value == _isAllowed) return;
                _isAllowed = value;
                AllowType allowType;
                if (!value.TryParse(out allowType))
                {
                    throw new AnycmdException("意外的AllowType:" + value);
                }
                this.AllowType = allowType;
            }
        }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        public AuditType AuditType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public AllowType AllowType { get; private set; }
    }
}
