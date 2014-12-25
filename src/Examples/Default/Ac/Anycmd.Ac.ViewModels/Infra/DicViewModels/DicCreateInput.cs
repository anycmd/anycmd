
namespace Anycmd.Ac.ViewModels.Infra.DicViewModels
{
    using Engine.Host.Ac.InOuts;
    using Model;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class DicCreateInput : EntityCreateInput, IInputModel, IDicCreateIo
    {
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
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
