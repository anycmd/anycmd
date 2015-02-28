
namespace Anycmd.Ac.ViewModels.EntityTypeViewModels
{
    using Engine.Ac.EntityTypes;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeCreateInput : EntityCreateInput, IEntityTypeCreateIo
    {
        public EntityTypeCreateInput()
        {
            HecpOntology = "EntityType";
            HecpVerb = "Create";
        }

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

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddEntityTypeCommand(acSession, this);
        }
    }
}
