
namespace Anycmd.Ac.ViewModels.AppSystemViewModels
{
    using Engine.Ac.AppSystems;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemCreateInput : EntityCreateInput, IAppSystemCreateIo
    {
        public AppSystemCreateInput()
        {
            HecpOntology = "AppSystem";
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
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SsoAuthAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid PrincipalId { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddAppSystemCommand(acSession, this);
        }
    }
}
