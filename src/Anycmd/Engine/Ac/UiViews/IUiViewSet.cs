
namespace Anycmd.Engine.Ac.UiViews
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是系统界面视图集。
    /// </summary>
    public interface IUiViewSet : IEnumerable<UiViewState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        bool TryGetUiView(FunctionState function, out UiViewState view);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        bool TryGetUiView(Guid viewId, out UiViewState view);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<UiViewButtonState> GetUiViewButtons();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        IReadOnlyList<UiViewButtonState> GetUiViewButtons(UiViewState view);
    }
}
