
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新系统实体类型时的输入或输出参数类型。
    /// </summary>
    public interface IEntityTypeUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Codespace { get; }
        bool IsCatalogued { get; }
        Guid DatabaseId { get; }
        string Description { get; }
        Guid DeveloperId { get; }
        int EditHeight { get; }
        int EditWidth { get; }
        string Name { get; }
        string SchemaName { get; }
        int SortCode { get; }
        string TableName { get; }
    }
}
