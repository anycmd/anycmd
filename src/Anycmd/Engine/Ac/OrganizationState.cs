
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Abstractions.Infra;
    using Host;
    using System;
    using Util;

    /// <summary>
    /// 表示组织结构业务实体。
    /// </summary>
    public sealed class OrganizationState : StateObject<OrganizationState>, IOrganization, IAcElement
    {
        public static readonly OrganizationState VirtualRoot = new OrganizationState(Guid.Empty)
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _categoryCode = string.Empty,
            _code = string.Empty,
            _createOn = SystemTime.MinDate,
            _description = string.Empty,
            _isEnabled = 1,
            _modifiedOn = null,
            _name = "虚拟根",
            _shortName = "根",
            _parentCode = null,
            _sortCode = 0,
            _contractorId = null
        };

        public static readonly OrganizationState Empty = new OrganizationState(Guid.Empty)
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _categoryCode = string.Empty,
            _code = string.Empty,
            _createOn = SystemTime.MinDate,
            _description = string.Empty,
            _isEnabled = 1,
            _modifiedOn = null,
            _name = "无",
            _shortName = "无",
            _parentCode = null,
            _contractorId = null,
            _sortCode = 0
        };

        private IAcDomain _acDomain;
        private string _code;
        private string _name;
        private string _shortName;
        private string _parentCode;
        private string _categoryCode;
        private Guid? _contractorId;
        private DateTime? _createOn;
        private DateTime? _modifiedOn;
        private string _description;
        private int _isEnabled;
        private int _sortCode;

        private OrganizationState(Guid id) : base(id) { }

        public static OrganizationState Create(IAcDomain host, OrganizationBase organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException("organization");
            }
            return new OrganizationState(organization.Id)
            {
                _acDomain = host,
                _categoryCode = organization.CategoryCode,
                _code = organization.Code,
                _createOn = organization.CreateOn,
                _description = organization.Description,
                _isEnabled = organization.IsEnabled,
                _modifiedOn = organization.ModifiedOn,
                _name = organization.Name,
                _shortName = organization.ShortName,
                _parentCode = organization.ParentCode,
                _sortCode = organization.SortCode,
                _contractorId = organization.ContractorId
            };
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Organization; }
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string ShortName
        {
            get { return _shortName; }
        }

        public string ParentCode
        {
            get { return _parentCode; }
        }

        public string CategoryCode
        {
            get { return _categoryCode; }
        }

        public Guid? ContractorId
        {
            get { return _contractorId; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public DateTime? ModifiedOn
        {
            get { return _modifiedOn; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        /// <summary>
        /// 返回Empty，不会返回null。
        /// 虚拟根的父级是Empty。Empty没有父级
        /// </summary>
        public OrganizationState Parent
        {
            get
            {
                if (this.Equals(OrganizationState.Empty))
                {
                    throw new InvalidOperationException("不能访问Null组织结构的父级");
                }
                if (this.Equals(OrganizationState.VirtualRoot))
                {
                    return OrganizationState.Empty;
                }
                if (string.IsNullOrEmpty(this.ParentCode))
                {
                    return OrganizationState.VirtualRoot;
                }
                OrganizationState parent;
                if (!AcDomain.OrganizationSet.TryGetOrganization(this.ParentCode, out parent))
                {
                    return OrganizationState.Empty;
                }

                return parent;
            }
        }

        protected override bool DoEquals(OrganizationState other)
        {
            return Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                ShortName == other.ShortName &&
                ParentCode == other.ParentCode &&
                CategoryCode == other.CategoryCode &&
                IsEnabled == other.IsEnabled &&
                ContractorId == other.ContractorId &&
                SortCode == other.SortCode &&
                Description == other.Description;
        }
    }
}
