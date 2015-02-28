
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class OntologyUpdateInput : IOntologyUpdateIo
    {
        public OntologyUpdateInput()
        {
            HecpOntology = "Ontology";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid MessageProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid EntityProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid EntityDatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid MessageDatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string EntitySchemaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string MessageSchemaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string EntityTableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EditWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EditHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateOntologyCommand(acSession, this);
        }
    }
}
