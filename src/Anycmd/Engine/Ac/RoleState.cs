
namespace Anycmd.Engine.Ac
{
    using Model;
    using Privileges;
    using Roles;
    using System;

    /// <summary>
    /// 表示角色业务实体。
    /// </summary>
    public sealed class RoleState : StateObject<RoleState>, IRole, IAcElement
    {
        private string _name;
        private string _categoryCode;
        private DateTime? _createOn;
        private int _isEnabled;
        private string _icon;
        private int _sortCode;

        private RoleState(Guid id) : base(id) { }

        public static RoleState Create(RoleBase role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return new RoleState(role.Id)
            {
                _createOn = role.CreateOn
            }.InternalModify(role);
        }

        internal RoleState InternalModify(RoleBase role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _name = role.Name;
            _categoryCode = role.CategoryCode;
            _isEnabled = role.IsEnabled;
            _icon = role.Icon;
            _sortCode = role.SortCode;

            return this;
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Role; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string CategoryCode
        {
            get { return _categoryCode; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public string Icon
        {
            get { return _icon; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Name:'{1}',
    CategoryCode:'{2}',
    CreateOn:;{3}',
    IsEnabled:{4},
    Icon:'{5}',
    SortCode:{6}
}}", Id, Name, CategoryCode, CreateOn, IsEnabled, Icon, SortCode);
        }

        protected override bool DoEquals(RoleState other)
        {
            return Id == other.Id &&
                Name == other.Name &&
                CategoryCode == other.CategoryCode &&
                IsEnabled == other.IsEnabled &&
                Icon == other.Icon &&
                SortCode == other.SortCode;
        }
    }
}
