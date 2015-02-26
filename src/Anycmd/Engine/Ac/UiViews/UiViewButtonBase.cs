
namespace Anycmd.Engine.Ac.UiViews
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 界面视图按钮基类。<see cref="IUiViewButton"/>
    /// </summary>
    public abstract class UiViewButtonBase : EntityBase, IUiViewButton
    {
        private Guid _uiViewId;
        private Guid _buttonId;

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FunctionId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UiViewId
        {
            get { return _uiViewId; }
            set
            {
                if (value == _uiViewId) return;
                if (_uiViewId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改所属界面视图");
                }
                _uiViewId = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid ButtonId
        {
            get { return _buttonId; }
            set
            {
                if (value == _buttonId) return;
                if (_buttonId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改关联按钮");
                }
                _buttonId = value;
            }
        }
    }
}
