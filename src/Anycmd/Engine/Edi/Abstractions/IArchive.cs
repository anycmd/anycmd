
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 记录每一次归档的实体。记录的信息如：什么时间归档的，归档的哪个本体，归档到了哪里（数据库），
    /// 以及归档标题和描述信息等。
    /// </summary>
    public interface IArchive
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        string RdbmsType { get; }
        #region Public EdiState Properties
        /// <summary>
        /// 源
        /// </summary>
        string DataSource { get; }
        /// <summary>
        /// 
        /// </summary>
        string FilePath { get; }
        /// <summary>
        /// 数字标识
        /// </summary>
        int NumberId { get; }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        string UserId { get; }
        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; }
        #endregion
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }
        /// <summary>
        /// 本体
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 归档时间
        /// </summary>
        DateTime ArchiveOn { get; }

        DateTime? CreateOn { get; }

        string CreateBy { get; }

        Guid? CreateUserId { get; }
    }
}
