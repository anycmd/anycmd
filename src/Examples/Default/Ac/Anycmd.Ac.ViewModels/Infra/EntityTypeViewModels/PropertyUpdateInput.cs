
namespace Anycmd.Ac.ViewModels.Infra.EntityTypeViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyUpdateInput : IPropertyUpdateIo
    {
        public PropertyUpdateInput()
        {
            OntologyCode = "Property";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ForeignPropertyId { get; set; }
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
        public Guid? DicId { get; set; }
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

        public UpdatePropertyCommand ToCommand()
        {
            return new UpdatePropertyCommand(this);
        }
    }
}
