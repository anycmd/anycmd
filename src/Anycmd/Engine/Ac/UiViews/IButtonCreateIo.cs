
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是创建系统按钮时的输入或输出参数类型。
    /// </summary>
    public interface IButtonCreateIo : IEntityCreateInput
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
