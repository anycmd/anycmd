
namespace Anycmd.Engine.Ac
{
    using Exceptions;
    using Model;
    using Privileges;
    using System;
    using UiViews;

    /// <summary>
    /// 表示系统菜单业务实体。
    /// </summary>
    public sealed class MenuState : StateObject<MenuState>, IMenu, IAcElement
    {
        private IAcDomain _acDomain;
        private Guid _appSystemId;
        private Guid? _parentId;
        private string _name;
        private string _url;
        private string _icon;
        private int _sortCode;
        private string _description;

        private MenuState(Guid id) : base(id) { }

        public static MenuState Create(IAcDomain acDomain, IMenu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }
            return new MenuState(menu.Id)
            {
                _acDomain = acDomain,
                _appSystemId = menu.AppSystemId
            }.InternalModify(menu);
        }

        internal MenuState InternalModify(IMenu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }
            if (!_acDomain.AppSystemSet.ContainsAppSystem(menu.AppSystemId))
            {
                throw new ValidationException("意外的应用系统标识" + menu.AppSystemId);
            }
            _name = menu.Name;
            _parentId = menu.ParentId;
            _url = menu.Url;
            _icon = menu.Icon;
            _sortCode = menu.SortCode;
            _description = menu.Description;

            return this;
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Menu; }
        }

        public Guid AppSystemId
        {
            get { return _appSystemId; }
        }

        public Guid? ParentId
        {
            get { return _parentId; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string Icon
        {
            get { return _icon; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public string Description
        {
            get { return _description; }
        }

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    AppSystemId:'{1}',
    ParentId:'{2}',
    Name:'{3}',
    Url:'{4}',
    Icon:'{5}',
    SortCode:{6},
    Description:'{7}'
}}", Id, AppSystemId, ParentId, Name, Url, Icon, SortCode, Description);
        }

        protected override bool DoEquals(MenuState other)
        {
            return Id == other.Id &&
                AppSystemId == other.AppSystemId &&
                ParentId == other.ParentId &&
                Name == other.Name &&
                Url == other.Url &&
                Icon == other.Icon &&
                SortCode == other.SortCode &&
                Description == other.Description;
        }
    }
}
