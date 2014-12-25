
namespace Anycmd.ViewModel
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 界面视图展示层模型。
    /// </summary>
    public class UiViewViewModel
    {
        public static readonly UiViewViewModel Empty = new UiViewViewModel(UiViewState.Empty, "无名页面");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        public UiViewViewModel(UiViewState page, string title)
        {
            this.UiView = page;
            this.Id = page.Id;
            this.Tooltip = page.Tooltip;
            this.Icon = page.Icon;
            this.Title = title;
        }

        public UiViewState UiView { get; private set; }

        public Guid Id { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 提示型帮助
        /// </summary>
        public string Tooltip { get; private set; }

        public string Icon { get; private set; }
    }
}
