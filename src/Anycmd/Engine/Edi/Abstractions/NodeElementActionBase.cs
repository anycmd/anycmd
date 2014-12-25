
namespace Anycmd.Engine.Edi.Abstractions {
    using Exceptions;
    using Model;
    using System;

    public abstract class NodeElementActionBase : EntityBase, INodeElementAction {
        private Guid _nodeId;
        private Guid _elementId;
        private Guid _actionId;

        protected NodeElementActionBase() { }

        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId {
            get { return _nodeId; }
            set {
                if (value == _nodeId) return;
                if (_nodeId != Guid.Empty) {
                    throw new CoreException("不能更改所属节点");
                }
                _nodeId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ElementId {
            get { return _elementId; }
            set {
                if (value == _elementId) return;
                if (_elementId != Guid.Empty) {
                    throw new CoreException("不能更改所属元素");
                }
                _elementId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ActionId {
            get { return _actionId; }
            set {
                if (value == _actionId) return;
                if (_actionId != Guid.Empty) {
                    throw new CoreException("不能更改所属动作");
                }
                _actionId = value;
            }
        }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        public bool IsAudit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
