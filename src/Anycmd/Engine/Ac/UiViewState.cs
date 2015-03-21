
namespace Anycmd.Engine.Ac
{
    using Host;
    using Model;
    using System;
    using UiViews;
    using Util;

    /// <summary>
    /// 界面视图业务实体类型。
    /// </summary>
    public sealed class UiViewState : StateObject<UiViewState>, IUiView
    {
        public static readonly UiViewState Empty = new UiViewState(Guid.Empty)
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _createOn = SystemTime.MinDate,
            _icon = string.Empty,
            _tooltip = string.Empty
        };

        private IAcDomain _acDomain;
        private string _tooltip;
        private string _icon;
        private DateTime? _createOn;

        private UiViewState(Guid id) : base(id) { }

        public static UiViewState Create(IAcDomain acDomain, UiViewBase view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            return new UiViewState(view.Id)
            {
                _acDomain = acDomain,
                _createOn = view.CreateOn
            }.InternalModify(view);
        }

        internal UiViewState InternalModify(UiViewBase view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }
            _tooltip = view.Tooltip;
            _icon = view.Icon;

            return this;
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public string Tooltip
        {
            get { return _tooltip; }
        }

        public string Icon
        {
            get { return _icon; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Tooltip:'{1}',
    Icon:'{2}',
    CreateOn:'{3}'
}}", Id, Tooltip, Icon, CreateOn);
        }

        protected override bool DoEquals(UiViewState other)
        {
            return Id == other.Id &&
                Tooltip == other.Tooltip &&
                Icon == other.Icon;
        }
    }
}
