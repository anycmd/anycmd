
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
        /// <summary>
        /// 用于定位空间的逻辑定位符。如“/User/Index”，不一定是定位到本菜单所属系统的内部的空间，
        /// 也可能定位到外部系统的空间，如“http://j.map.baidu.com/u3fgA”。
        /// </summary>
        string Url { get; }
    }
}
