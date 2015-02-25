
namespace Anycmd.Engine.Ac.EntityTypes
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是系统实体类型。
    /// <remarks>该模型是程序开发模型，被程序员使用，用户不关心本概念。</remarks>
    /// </summary>
    public interface IEntityType
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 编码空间
        /// </summary>
        string Codespace { get; }
        /// <summary>
        /// 模型编码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 模型名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsCatalogued { get; }
        /// <summary>
        /// 模型对应的存储数据库
        /// </summary>
        Guid DatabaseId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid DeveloperId { get; }
        /// <summary>
        /// 库模式
        /// </summary>
        string SchemaName { get; }
        /// <summary>
        /// 表名
        /// </summary>
        string TableName { get; }
        /// <summary>
        /// 排序
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 界面配置项。编辑窗体的宽度
        /// </summary>
        int EditWidth { get; }
        /// <summary>
        /// 界面配置项。编辑窗体的高徒
        /// </summary>
        int EditHeight { get; }
    }
}
