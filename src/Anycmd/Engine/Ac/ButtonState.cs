
namespace Anycmd.Engine.Ac
{
    using Model;
    using System;
    using UiViews;

    /// <summary>
    /// 表示按钮业务实体。
    /// </summary>
    public sealed class ButtonState : StateObject<ButtonState>, IButton
    {
        private string _name;
        private string _code;
        private string _categoryCode;
        private string _icon;
        private int _sortCode;
        private int _isEnabled;
        private DateTime? _createOn;

        private ButtonState(Guid id) : base(id) { }

        public static ButtonState Create(ButtonBase button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button");
            }
            return new ButtonState(button.Id)
            {
                _createOn = button.CreateOn
            }.InternalModify(button);
        }

        internal ButtonState InternalModify(ButtonBase button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button");
            }
            _name = button.Name;
            _categoryCode = button.CategoryCode;
            _code = button.Code;
            _icon = button.Icon;
            _sortCode = button.SortCode;
            _isEnabled = button.IsEnabled;

            return this;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode
        {
            get { return _categoryCode; }
        }

        public string Icon
        {
            get { return _icon; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
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
    Name:'{1}',
    Code:'{2}',
    CategoryCode:'{3}',
    Icon:'{4}',
    SortCode:{5},
    IsEnabled:{6},
    CreateOn:'{7}'
}}", Id, Name, Code, CategoryCode, Icon, SortCode, IsEnabled, CreateOn);
        }

        protected override bool DoEquals(ButtonState other)
        {
            return Id == other.Id &&
                Name == other.Name &&
                Code == other.Code &&
                CategoryCode == other.CategoryCode &&
                Icon == other.Icon &&
                SortCode == other.SortCode &&
                IsEnabled == other.IsEnabled;
        }
    }
}
