
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 进程。
    /// 如不明确说明，“进程”所处的默认上下文是基础库系统。 “进程”指的是“命令执行器”、“命令分发器”、
    /// “Mis系统”、“Web服务系统”这四类操作系统进程，这里的进程管理不是操作系统层级的系统管理
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// 进程标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 进程类型
        /// </summary>
        string Type { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        int NetPort { get; }
        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 有效状态
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 本体标识
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 目录码
        /// </summary>
        string CatalogCode { get; }

        DateTime? CreateOn { get; }
    }
}
