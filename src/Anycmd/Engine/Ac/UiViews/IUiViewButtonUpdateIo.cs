
namespace Anycmd.Engine.Ac.UiViews
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新界面视图按钮时的输入或输出参数类型。
    /// </summary>
    public interface IUiViewButtonUpdateIo : IEntityUpdateInput
    {
        Guid? FunctionId { get; }
        int IsEnabled { get; }
    }
}
