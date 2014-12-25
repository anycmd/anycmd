
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Model;
    using System;

    /// <summary>
    /// 表示组业务实体。
    /// </summary>
    public sealed class GroupState : StateObject<GroupState>, IGroup, IStateObject
    {
        private string _name;
        private string _organizationCode;
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
                _name = group.Name,
                _organizationCode = group.OrganizationCode,
                _categoryCode = group.CategoryCode,
                _sortCode = group.SortCode,
                _isEnabled = group.IsEnabled,
                _createOn = group.CreateOn
            };
        }

        public string Name
        {
            get { return _name; }
        }

        public string OrganizationCode
        {
            get { return _organizationCode; }
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
                OrganizationCode == other.OrganizationCode &&
                CategoryCode == other.CategoryCode &&
                SortCode == other.SortCode &&
                IsEnabled == other.IsEnabled;
        }
    }
}
