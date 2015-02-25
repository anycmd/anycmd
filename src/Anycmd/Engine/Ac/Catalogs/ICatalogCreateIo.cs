
namespace Anycmd.Engine.Ac.Catalogs
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是创建目录时的输入或输出参数类型。
    /// </summary>
    public interface ICatalogCreateIo : IEntityCreateInput
    {
        string CategoryCode { get; }
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        string ParentCode { get; }
        int SortCode { get; }
    }
}
