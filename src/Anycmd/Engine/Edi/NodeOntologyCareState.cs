
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class NodeOntologyCareState : StateObject<NodeOntologyCareState>, INodeOntologyCare, IStateObject
    {
        private NodeOntologyCareState(Guid id) : base(id) { }

        public static NodeOntologyCareState Create(INodeOntologyCare nodeOntologyCare)
        {
            if (nodeOntologyCare == null)
            {
                throw new ArgumentNullException("nodeOntologyCare");
            }
            return new NodeOntologyCareState(nodeOntologyCare.Id)
            {
                CreateOn = nodeOntologyCare.CreateOn,
                NodeId = nodeOntologyCare.NodeId,
                OntologyId = nodeOntologyCare.OntologyId
            };
        }

        public Guid NodeId { get; private set; }

        public Guid OntologyId { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(NodeOntologyCareState other)
        {
            return
                Id == other.Id &&
                NodeId == other.NodeId &&
                OntologyId == other.OntologyId;
        }
    }
}
