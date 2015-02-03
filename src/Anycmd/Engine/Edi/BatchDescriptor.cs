
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using System;
    using Util;

    /// <summary>
    /// “批”描述对象。
    /// </summary>
    public class BatchDescriptor
    {
        private readonly IBatch _batch;
        private readonly BatchType _batchType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="batch"></param>
        public BatchDescriptor(IAcDomain acDomain, IBatch batch)
        {
            if (batch == null)
            {
                throw new ArgumentNullException("batch");
            }
            this._batch = batch;
            OntologyDescriptor ontology;
            if (!acDomain.NodeHost.Ontologies.TryGetOntology(batch.OntologyId, out ontology))
            {
                throw new AnycmdException("意外的本体标识" + batch.OntologyId);
            }
            this.Ontology = ontology;
            NodeDescriptor node;
            if (!acDomain.NodeHost.Nodes.TryGetNodeById(batch.NodeId.ToString(), out node))
            {
                throw new AnycmdException("意外的节点标识" + batch.NodeId);
            }
            this.Node = node;
            if (!batch.Type.TryParse(out _batchType))
            {
                throw new AnycmdException("意外的批类型" + batch.Type);
            }
        }

        /// <summary>
        /// 批类型
        /// </summary>
        public BatchType Type
        {
            get { return _batchType; }
        }

        /// <summary>
        /// 本体
        /// </summary>
        public OntologyDescriptor Ontology
        {
            get;
            private set;
        }

        /// <summary>
        /// 可能为null
        /// </summary>
        public NodeDescriptor Node
        {
            get;
            private set;
        }
    }
}
