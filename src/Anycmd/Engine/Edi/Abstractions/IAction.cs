
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 本体动作。
    /// 动作用以定义可以面向具体本体做些什么。如，可以创建教师、可以修改教师的信息、可以删除教师，
    /// 所以教师本体上定义有编码为“Create”、“Update”、“Delete”的动作。动作是依赖于本体的，如果
    /// 本体是“文档”则动作编码为“Create”、“Update”、“Delete”不再合适，“Upload”、“Download”、“Compress”、“UnCompress”更合适。
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 本体标识
        /// </summary>
        Guid OntologyId { get; }

        /// <summary>
        /// 动作码
        /// </summary>
        string Verb { get; }

        /// <summary>
        /// 动作名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否允许
        /// </summary>
        string IsAllowed { get; }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        string IsAudit { get; }

        /// <summary>
        /// 是否持久化到数据库
        /// </summary>
        bool IsPersist { get; }

        int SortCode { get; }

        string Description { get; }

        DateTime? CreateOn { get; }
    }
}
