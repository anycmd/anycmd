
namespace Anycmd.Ac.ViewModels.MenuViewModels
{
    using Engine.Ac.UiViews;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class MenuCreateInput : EntityCreateInput, IMenuCreateIo
    {
        public MenuCreateInput()
        {
            HecpOntology = "Menu";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid AppSystemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddMenuCommand(acSession, this);
        }
    }
}
