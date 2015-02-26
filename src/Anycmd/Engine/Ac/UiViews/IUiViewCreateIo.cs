
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是创建界面视图时的输入或输出参数类型。
    /// </summary>
    public interface IUiViewCreateIo : IEntityCreateInput
    {
        string Icon { get; }
        string Tooltip { get; }
    }
}
