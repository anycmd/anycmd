
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是更新界面视图时的输入或输出参数类型。
    /// </summary>
    public interface IUiViewUpdateIo : IEntityUpdateInput
    {
        string Icon { get; }
        string Tooltip { get; }
    }
}
