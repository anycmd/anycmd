
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Host.Ac;
    using Model;
    using System;
    using Util;

    public sealed class CatalogAction : EntityBase, ICatalogAction
    {
        private Guid _catalogId;
        private Guid _actionId;
        private string _isAudit;
        private string _isAllowed;

        public CatalogAction() { }

        /// <summary>
        /// 
        /// </summary>
        public Guid CatalogId
        {
            get { return _catalogId; }
            set
            {
                if (value == _catalogId) return;
                if (_catalogId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改所属目录");
                }
                _catalogId = value;
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
        /// 是否需要审核
        /// </summary>
        public AuditType AuditType { get; private set; }

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
        /// 
        /// </summary>
        public AllowType AllowType { get; private set; }
    }
}
