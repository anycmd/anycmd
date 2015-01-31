
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 定义本体模型。本体是指一种“形式化的，对于共享概念体系的明确而又详细的说明”。进一步的了解可查询维基百科或百度百科，本协议中使用“本体”这个词汇时使用的是百科上的内涵。
    /// <remarks>
    /// 为什么是接口？使用接口将其约束为不可变模型，从而使插件开发者不能正常修改它。
    /// </remarks>
    /// </summary>
    public interface IOntology
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        string Triggers { get; }
        /// <summary>
        /// 
        /// </summary>
        string Icon { get; }
        /// <summary>
        /// 
        /// </summary>
        bool ServiceIsAlive { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid MessageProviderId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid EntityProviderId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid EntityDatabaseId { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsSystem { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsCataloguedEntity { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsLogicalDeletionEntity { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid MessageDatabaseId { get; }
        /// <summary>
        /// 
        /// </summary>
        int ReceivedMessageBufferSize { get; }
        /// <summary>
        /// 
        /// </summary>
        string EntitySchemaName { get; }
        /// <summary>
        /// 
        /// </summary>
        string MessageSchemaName { get; }
        /// <summary>
        /// 
        /// </summary>
        string EntityTableName { get; }
        /// <summary>
        /// 待执行命令提供程序批幅
        /// </summary>
        int ExecutorLoadCount { get; }
        /// <summary>
        /// 命令执行器休眠时间段长度
        /// </summary>
        int ExecutorSleepTimeSpan { get; }
        /// <summary>
        /// 待分发命令提供程序批幅
        /// </summary>
        int DispatcherLoadCount { get; }
        /// <summary>
        /// 命令分发器休眠时间段长度
        /// </summary>
        int DispatcherSleepTimeSpan { get; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        bool CanAction { get; }
        /// <summary>
        /// 
        /// </summary>
        bool CanCommand { get; }
        /// <summary>
        /// 
        /// </summary>
        bool CanEvent { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; }
    }
}
