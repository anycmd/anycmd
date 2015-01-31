
namespace Anycmd.Engine.Edi.Abstractions
{
    using Model;
    using System;

    /// <summary>
    /// 批。
    /// 主要用在初始化数据的场景。目前的批模块实现的是为符合给定条件的实体批量生成指定类型的命令。
    /// 比如：给定条件“本体是教师”、“某某目录的教师”、“包括或不包括目录的下级目录的教师”、
    /// “最后修改时间范围”等可以筛选出来一批教师，指定“批类型”是“BuildCreateCommand”则批量生成Create型命令，
    /// 生成的命令放入分发队列则“命令分发器”自动分发。
    /// 什么是“批”？基础库从两个角度解释“批”：
    /// 1，一个批事务的内部往往包含多步逻辑，虽然对外只表现为一个“执行”按钮。
    /// 2，批的执行往往影响多条“实体”或“命令”。说明：如不明确说明，周报的默认上下文都是基础库“实体”
    /// 指的为每一条教师、学生这样的数据，“命令”指的是完整的描述了一次对实体的操作的对象。
    /// </summary>
    public interface IBatch : IEntity
    {
        /// <summary>
        /// 本体标识
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 节点标识
        /// </summary>
        Guid NodeId { get; }
        /// <summary>
        /// 批类型码
        /// </summary>
        string Type { get; }
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }
        /// <summary>
        /// 目录码
        /// </summary>
        string CatalogCode { get; }
        /// <summary>
        /// 是否包括子孙级目录下的实体
        /// </summary>
        bool? IncludeDescendants { get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        int Total { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateOn { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid? CreateUserId { get; }
        /// <summary>
        /// 
        /// </summary>
        string CreateBy { get; }
    }
}
