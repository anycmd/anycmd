
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Exceptions;
    using Model;
    using System;

    public sealed class UiViewButtonState : StateObject<UiViewButtonState>, IUiViewButton
    {
        private IAcDomain _acDomain;
        private Guid _viewId;
        private Guid _buttonId;
        private Guid? _functionId;
        private int _isEnabled;
        private DateTime? _createOn;

        private UiViewButtonState(Guid id) : base(id) { }

        public static UiViewButtonState Create(IAcDomain acDomain, UiViewButtonBase viewButton)
        {
            if (viewButton == null)
            {
                throw new ArgumentNullException("viewButton");
            }
            UiViewState view;
            if (!acDomain.UiViewSet.TryGetUiView(viewButton.UiViewId, out view))
            {
                throw new AnycmdException("意外的界面视图" + viewButton.UiViewId);
            }
            ButtonState button;
            if (!acDomain.ButtonSet.TryGetButton(viewButton.ButtonId, out button))
            {
                throw new AnycmdException("意外的按钮" + viewButton.ButtonId);
            }
            var functionId = viewButton.FunctionId;
            if (functionId == Guid.Empty)
            {
                functionId = null;
            }
            if (!functionId.HasValue)
                return new UiViewButtonState(viewButton.Id)
                {
                    _acDomain = acDomain,
                    _viewId = viewButton.UiViewId,
                    _functionId = null,
                    _buttonId = viewButton.ButtonId,
                    _isEnabled = viewButton.IsEnabled,
                    _createOn = viewButton.CreateOn
                };
            FunctionState function;
            if (!acDomain.FunctionSet.TryGetFunction(functionId.Value, out function))
            {
                throw new ValidationException("意外的功能标识" + functionId);
            }
            return new UiViewButtonState(viewButton.Id)
            {
                _acDomain = acDomain,
                _viewId = viewButton.UiViewId,
                _functionId = functionId,
                _buttonId = viewButton.ButtonId,
                _isEnabled = viewButton.IsEnabled,
                _createOn = viewButton.CreateOn
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid UiViewId
        {
            get { return _viewId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid ButtonId
        {
            get { return _buttonId; }
        }

        public Guid? FunctionId
        {
            get { return _functionId; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonState Button
        {
            get
            {
                ButtonState button;
                if (!_acDomain.ButtonSet.TryGetButton(this.ButtonId, out button))
                {
                    throw new AnycmdException("意外的按钮" + this.ButtonId);
                }
                return button;
            }
        }

        public UiViewState UiView
        {
            get
            {
                UiViewState view;
                if (!_acDomain.UiViewSet.TryGetUiView(this.UiViewId, out view))
                {
                    throw new AnycmdException("意外的界面视图按钮界面视图标识" + this.UiViewId);
                }
                return view;
            }
        }

        protected override bool DoEquals(UiViewButtonState other)
        {
            return Id == other.Id &&
                UiViewId == other.UiViewId &&
                FunctionId == other.FunctionId &&
                ButtonId == other.ButtonId &&
                IsEnabled == other.IsEnabled;
        }
    }
}
