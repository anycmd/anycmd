
namespace Anycmd.Ac.ViewModels.EntityTypeViewModels
{
    using Engine.Ac.EntityTypes;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeUpdateInput : IEntityTypeUpdateIo
    {
        public EntityTypeUpdateInput()
        {
            HecpOntology = "EntityType";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsCatalogued { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid DatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SchemaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Codespace { get; set; }
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
        [Required]
        public Guid DeveloperId { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateEntityTypeCommand(acSession, this);
        }
    }
}
