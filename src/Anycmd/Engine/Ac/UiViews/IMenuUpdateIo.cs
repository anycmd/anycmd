
namespace Anycmd.Engine.Ac.UiViews
{
    using InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新系统菜单时的输入或输出参数类型。
    /// </summary>
    public interface IMenuUpdateIo : IEntityUpdateInput
    {
        Guid AppSystemId { get; }
        string Description { get; }
        string Icon { get; }
        string Name { get; }
        int SortCode { get; }
        string Url { get; }
    }
}
