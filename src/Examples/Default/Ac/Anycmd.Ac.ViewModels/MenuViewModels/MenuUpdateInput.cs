
namespace Anycmd.Ac.ViewModels.MenuViewModels
{
    using Engine.Ac.UiViews;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class MenuUpdateInput : IMenuUpdateIo
    {
        public MenuUpdateInput()
        {
            HecpOntology = "Menu";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
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

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateMenuCommand(acSession, this);
        }
    }
}
