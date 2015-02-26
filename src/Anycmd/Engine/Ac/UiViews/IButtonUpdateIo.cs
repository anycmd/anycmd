
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是更新系统按钮时的输入或输出参数类型。
    /// </summary>
    public interface IButtonUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string CategoryCode { get; }
        string Description { get; }
        string Icon { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
