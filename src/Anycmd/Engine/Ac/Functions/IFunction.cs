
namespace Anycmd.Engine.Ac.Functions
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是操作、功能、函数、逻辑单元。一个Function标识的是一个输入输入装置，是一个逻辑单元。
    /// <remarks>
    /// 功能是一个程序的可执行映像，被调用时能为用户执行某些引起系统状态变化的操作
    /// （判定是否引起了系统变化的标准是什么？需要具体分析，如果系统中有“访问计数”
    /// 功能的话，那么只读的操作也是引起了系统的状态变化的）。
    /// </remarks>
    /// </summary>
    public interface IFunction
    {
        Guid Id { get; }
        /// <summary>
        /// 所属资源
        /// </summary>
        Guid ResourceTypeId { get; }
        /// <summary>
        /// 托管标识
        /// </summary>
        Guid? Guid { get; }
        /// <summary>
        /// 操作码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsManaged { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid DeveloperId { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 操作说明
        /// </summary>
        string Description { get; }
    }
}
