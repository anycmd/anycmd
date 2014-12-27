
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示系统界面视图数据访问实体。
    /// </summary>
    public class UiView : UiViewBase, IAggregateRoot
    {
        public UiView() { }

        public static UiView Create(IUiViewCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new UiView
            {
                Id = input.Id.Value,
                Icon = input.Icon,
                Tooltip = input.Tooltip
            };
        }

        public void Update(IUiViewUpdateIo input)
        {
            this.Tooltip = input.Tooltip;
            this.Icon = input.Icon;
        }
    }
}
