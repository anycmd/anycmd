
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Host.Edi.InOuts;
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class NodeOntologyCareCreateInput : ManagedPropertyValues, IInputModel, INodeOntologyCareCreateIo
    {
        private Guid? _id = null;

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
    }
}
