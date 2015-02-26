
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建系统菜单时的输入或输出参数类型。
    /// </summary>
    public interface IMenuCreateIo : IEntityCreateInput
    {
        Guid AppSystemId { get; }
        string Description { get; }
        string Icon { get; }
        string Name { get; }
        Guid? ParentId { get; }
        int SortCode { get; }
        string Url { get; }
    }
}
