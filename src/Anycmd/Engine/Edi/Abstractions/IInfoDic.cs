
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 信息字典。有些本体元素在实体上的取值不是任意的。当本体是“人”时，人有“民族”这个本体元素，
    /// 本体元素“民族”的“数据类型”是字典型的。“教师”本体是“人”本体的一个子类，张老师是一个“实体人”，
    /// 张老师的“民族属性”取值就不是任意的而是由教育部的“民族”字典限定的。
    /// <remarks>
    /// 为什么是接口？信息字典将被提供给插件编写者所以需要是不可变的对象，所以这里抽象出一个只读的接口。
    /// </remarks>
    /// </summary>
    public interface IInfoDic
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        #region Public Edi Properties
        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 有效标记
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        #endregion

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        DateTime? CreateOn { get; }
    }
}
