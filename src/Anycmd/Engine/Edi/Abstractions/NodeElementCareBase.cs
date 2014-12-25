
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class NodeElementCareBase : EntityBase, INodeElementCare
    {
        #region Private Fields
        private Guid _nodeId;
        private Guid _elementId;

        #endregion

        #region Public Properties
        /// <summary>
        /// 节点Id
        /// </summary>
        public Guid NodeId
        {
            get { return _nodeId; }
            set
            {
                if (value == _nodeId) return;
                if (_nodeId != Guid.Empty)
                {
                    throw new CoreException("关联节点不能更改");
                }
                _nodeId = value;
            }
        }
        /// <summary>
        /// 本体元素主键
        /// </summary>
        public Guid ElementId
        {
            get { return _elementId; }
            set
            {
                if (value == _elementId) return;
                if (_elementId != Guid.Empty)
                {
                    throw new CoreException("关联本体元素不能更改");
                }
                _elementId = value;
            }
        }

        /// <summary>
        /// 是否是信息标识本体元素
        /// </summary>
        public bool IsInfoIdItem { get; set; }

        #endregion
    }
}
