
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementCareCreateInput : EntityCreateInput, INodeElementCareCreateIo
    {
        public NodeElementCareCreateInput()
        {
            HecpOntology = "NodeElementCare";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid ElementId { get; set; }

        public bool IsInfoIdItem { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddNodeElementCareCommand(acSession, this);
        }
    }
}
