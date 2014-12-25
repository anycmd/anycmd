
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class NodeElementActionState : StateObject<NodeElementActionState>, INodeElementAction, IStateObject
    {
        private NodeElementActionState(Guid id) : base(id) { }

        public static NodeElementActionState Create(INodeElementAction nodeElementAction)
        {
            if (nodeElementAction == null)
            {
                throw new ArgumentNullException("nodeElementAction");
            }
            return new NodeElementActionState(nodeElementAction.Id)
            {
                ActionId = nodeElementAction.ActionId,
                CreateOn = nodeElementAction.CreateOn,
                ElementId = nodeElementAction.ElementId,
                IsAllowed = nodeElementAction.IsAllowed,
                IsAudit = nodeElementAction.IsAudit,
                NodeId = nodeElementAction.NodeId
            };
        }

        public Guid ActionId { get; private set; }

        public Guid ElementId { get; private set; }

        public bool IsAllowed { get; private set; }

        public bool IsAudit { get; private set; }

        public Guid NodeId { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(NodeElementActionState other)
        {
            return ActionId == other.ActionId && 
                CreateOn == other.CreateOn && 
                ElementId == other.ElementId && 
                Id == other.Id && 
                IsAllowed == other.IsAllowed &&
                IsAudit == other.IsAudit && 
                NodeId == other.NodeId;
        }
    }
}
