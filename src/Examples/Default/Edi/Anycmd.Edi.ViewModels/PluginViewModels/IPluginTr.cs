
namespace Anycmd.Edi.ViewModels.PluginViewModels
{
    using System;

    public interface IPluginTr
    {
        /// <summary>
        /// 
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string AuthorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; set; }
    }
}
