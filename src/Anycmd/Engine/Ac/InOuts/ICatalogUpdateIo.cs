
namespace Anycmd.Engine.Ac.InOuts
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新目录时的输入或输出参数类型。
    /// </summary>
    public interface ICatalogUpdateIo : IEntityUpdateInput
    {
        string Address { get; }
        string CategoryCode { get; }
        string Code { get; }
        string Description { get; }
        string Fax { get; }
        string Icon { get; }
        string InnerPhone { get; }
        int IsEnabled { get; }
        string Name { get; }
        string Postalcode { get; }
        string OuterPhone { get; }
        string ParentCode { get; }
        string ShortName { get; }
        int SortCode { get; }
        string WebPage { get; }
        Guid? ContractorId { get; }
    }
}
