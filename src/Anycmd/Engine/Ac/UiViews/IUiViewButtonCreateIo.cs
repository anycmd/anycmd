
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建界面视图按钮时的输入或输出参数类型。
    /// </summary>
    public interface IUiViewButtonCreateIo : IEntityCreateInput
    {
        Guid ButtonId { get; }
        Guid? FunctionId { get; }
        int IsEnabled { get; }
        Guid UiViewId { get; }
    }
}
