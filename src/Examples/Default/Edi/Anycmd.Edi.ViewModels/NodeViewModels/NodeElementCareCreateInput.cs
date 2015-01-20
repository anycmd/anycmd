
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
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

        public override IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new AddNodeElementCareCommand(userSession, this);
        }
    }
}
