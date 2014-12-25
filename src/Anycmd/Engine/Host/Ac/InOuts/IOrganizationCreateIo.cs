
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建组织结构时的输入或输出参数类型。
    /// </summary>
    public interface IOrganizationCreateIo : IEntityCreateInput
    {
        string Address { get; }
        string CategoryCode { get; }
        string Code { get; }
        string Description { get; }
        string Fax { get; }
        string Icon { get; }
        Guid? ContractorId { get; }
        string InnerPhone { get; }
        int IsEnabled { get; }
        string Name { get; }
        string OuterPhone { get; }
        string ParentCode { get; }
        string PostalCode { get; }
        string ShortName { get; }
        int SortCode { get; }
        string WebPage { get; }
    }
}
