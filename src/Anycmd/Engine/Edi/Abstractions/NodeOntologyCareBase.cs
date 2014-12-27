
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class NodeOntologyCareBase : EntityBase, INodeOntologyCare
    {
        #region Private Fields
        private Guid _nodeId;
        private Guid _ontologyId;
        #endregion

        protected NodeOntologyCareBase() { }

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
                    throw new AnycmdException("关联节点不能更改");
                }
                _nodeId = value;
            }
        }
        /// <summary>
        /// 本体主键
        /// </summary>
        public Guid OntologyId
        {
            get { return _ontologyId; }
            set
            {
                if (value == _ontologyId) return;
                if (_ontologyId != Guid.Empty)
                {
                    throw new AnycmdException("关联本体不能更改");
                }
                _ontologyId = value;
            }
        }
        #endregion
    }
}
