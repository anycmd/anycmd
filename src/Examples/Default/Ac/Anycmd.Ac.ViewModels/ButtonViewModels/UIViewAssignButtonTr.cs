
namespace Anycmd.Ac.ViewModels.ButtonViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewAssignButtonTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? FunctionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FunctionCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UiViewId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ButtonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ButtonIsEnabled { get; set; }

        public int FunctionIsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAssigned { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
