
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class NodeElementCareState : StateObject<NodeElementCareState>, INodeElementCare, IStateObject
    {
        private NodeElementCareState(Guid id) : base(id) { }

        public static NodeElementCareState Create(INodeElementCare nodeElementCare)
        {
            if (nodeElementCare == null)
            {
                throw new ArgumentNullException("nodeElementCare");
            }
            return new NodeElementCareState(nodeElementCare.Id)
            {
                ElementId = nodeElementCare.ElementId,
                IsInfoIdItem = nodeElementCare.IsInfoIdItem,
                NodeId = nodeElementCare.NodeId
            };
        }

        public Guid ElementId { get; private set; }

        public Guid NodeId { get; private set; }

        public bool IsInfoIdItem { get; private set; }

        protected override bool DoEquals(NodeElementCareState other)
        {
            return ElementId == other.ElementId &&
                Id == other.Id &&
                IsInfoIdItem == other.IsInfoIdItem &&
                NodeId == other.NodeId;
        }
    }
}
