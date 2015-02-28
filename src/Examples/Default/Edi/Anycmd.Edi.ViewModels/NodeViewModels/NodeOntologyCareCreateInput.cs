
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class NodeOntologyCareCreateInput : ManagedPropertyValues, INodeOntologyCareCreateIo
    {
        private Guid? _id = null;

        public NodeOntologyCareCreateInput()
        {
            this.HecpOntology = "NodeOntologyCare";
            this.HecpVerb = "Create";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid? Id
        {
            get
            {
                if (_id == null)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set { _id = value; }
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
        public Guid OntologyId { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddNodeOntologyCareCommand(acSession, this);
        }
    }
}
