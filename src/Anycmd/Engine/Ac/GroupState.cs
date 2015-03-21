
namespace Anycmd.Engine.Ac
{
    using Groups;
    using Model;
    using Privileges;
    using System;

    /// <summary>
    /// 表示组业务实体。
    /// </summary>
    public sealed class GroupState : StateObject<GroupState>, IGroup, IAcElement
    {
        private string _name;
        private string _categoryCode;
        private int _sortCode;
        private int _isEnabled;
        private DateTime? _createOn;

        private GroupState(Guid id) : base(id) { }

        public static GroupState Create(GroupBase group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            return new GroupState(group.Id)
            {
                _createOn = group.CreateOn
            }.InternalModify(group);
        }

        internal GroupState InternalModify(GroupBase group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            _name = group.Name;
            _categoryCode = group.CategoryCode;
            _sortCode = group.SortCode;
            _isEnabled = group.IsEnabled;

            return this;
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Group; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string CategoryCode
        {
            get { return _categoryCode; }
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

        protected override bool DoEquals(GroupState other)
        {
            return Id == other.Id &&
                Name == other.Name &&
                CategoryCode == other.CategoryCode &&
                SortCode == other.SortCode &&
                IsEnabled == other.IsEnabled;
        }
    }
}
