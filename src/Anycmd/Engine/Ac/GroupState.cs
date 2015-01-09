
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using System;

    /// <summary>
    /// 表示组业务实体。
    /// </summary>
    public sealed class GroupState : StateObject<GroupState>, IGroup, IAcElement
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

        public AcElementType AcElementType
        {
            get { return AcElementType.Group; }
        }

        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 组所属组织结构，如果该属性有指向组织结构的值的话该组就是绑定了组织结构的，比如岗位就属于绑定了组织结构的组。绑定了组织结构的工作组中的资源只能来自于这个组织结构和其子组织结构。
        /// <remarks>
        /// 工作组是组中有主体的组。工作组是跨组织结构的资源组，组中的资源不只来自一个组织结构。
        /// </remarks>
        /// </summary>
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
