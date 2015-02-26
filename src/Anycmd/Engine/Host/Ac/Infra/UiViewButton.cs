
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.UiViews;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示系统界面视图按钮数据访问实体，设计用于支持自动化界面等。将界面视图和按钮的关系视作实体。
    /// <remarks>
    /// 1该模型是程序开发模型，被程序员使用，用户不关心本概念;
    /// 2在数据库中没有任何表需要引用UIViewButton表所以UIViewButton无需标记删除
    /// </remarks>
    /// </summary>
    public class UiViewButton : UiViewButtonBase, IAggregateRoot
    {
        #region Ctor
        public UiViewButton()
        {
            base.IsEnabled = 1;
        }
        #endregion

        public static UiViewButton Create(IUiViewButtonCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new UiViewButton
                {
                    Id = input.Id.Value,
                    ButtonId = input.ButtonId,
                    UiViewId = input.UiViewId,
                    FunctionId = input.FunctionId,
                    IsEnabled = input.IsEnabled
                };
        }

        public void Update(IUiViewButtonUpdateIo input)
        {
            this.IsEnabled = input.IsEnabled;
            this.FunctionId = input.FunctionId;
        }
    }
}
