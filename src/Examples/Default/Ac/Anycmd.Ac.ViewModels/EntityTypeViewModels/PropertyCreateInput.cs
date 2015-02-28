
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
    public class PropertyCreateInput : EntityCreateInput, IPropertyCreateIo
    {
        public PropertyCreateInput()
        {
            HecpOntology = "Property";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid EntityTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ForeignPropertyId { get; set; }
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
        public int? MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GuideWords { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? DicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDetailsShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeveloperOnly { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsInput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTotalLine { get; set; }


        public string GroupCode { get; set; }

        public string Tooltip { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddPropertyCommand(acSession, this);
        }
    }
}
