
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 信息字典项
    /// </summary>
    public interface IInfoDicItem
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        #region Public Edi Properties
        /// <summary>
        /// 字典值级别
        /// </summary>
        string Level { get; }
        /// <summary>
        /// 本地方言编码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 有效标记
        /// </summary>
        int IsEnabled { get; }
        #endregion
        /// <summary>
        /// 字典主键
        /// </summary>
        Guid InfoDicId { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? ModifiedOn { get; }
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }
    }
}
