
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Abstractions.Infra;
    using Host;
    using System;
    using Util;

    /// <summary>
    /// 表示目录业务实体。
    /// </summary>
    public sealed class CatalogState : StateObject<CatalogState>, ICatalog, IAcElement
    {
        public static readonly CatalogState VirtualRoot = new CatalogState(Guid.Empty)
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

        public static readonly CatalogState Empty = new CatalogState(Guid.Empty)
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

        private CatalogState(Guid id) : base(id) { }

        public static CatalogState Create(IAcDomain host, CatalogBase catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException("catalog");
            }
            return new CatalogState(catalog.Id)
            {
                _acDomain = host,
                _categoryCode = catalog.CategoryCode,
                _code = catalog.Code,
                _createOn = catalog.CreateOn,
                _description = catalog.Description,
                _isEnabled = catalog.IsEnabled,
                _modifiedOn = catalog.ModifiedOn,
                _name = catalog.Name,
                _shortName = catalog.ShortName,
                _parentCode = catalog.ParentCode,
                _sortCode = catalog.SortCode,
                _contractorId = catalog.ContractorId
            };
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Catalog; }
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        /// <summary>
        /// 返回Empty，不会返回null。
        /// 虚拟根的父级是Empty。Empty没有父级
        /// </summary>
        public CatalogState Parent
        {
            get
            {
                if (this.Equals(Empty))
                {
                    throw new InvalidOperationException("不能访问Null目录的父级");
                }
                if (this.Equals(VirtualRoot))
                {
                    return Empty;
                }
                if (string.IsNullOrEmpty(this.ParentCode))
                {
                    return CatalogState.VirtualRoot;
                }
                CatalogState parent;
                if (!AcDomain.CatalogSet.TryGetCatalog(this.ParentCode, out parent))
                {
                    return Empty;
                }

                return parent;
            }
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

        /// <summary>
        /// 包工头
        /// </summary>
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

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Code:'{1}',
    Name:'{2}',
    ShortName:'{3}',
    ParentCode:'{4}',
    CategoryCode:'{5}',
    ContractorId:'{6}'，
    CreateOn:'{7}',
    ModifiedOn:'{8}',
    Description:'{9}',
    IsEnabled:{10},
    SortCode:{11}
}}", Id, Code, Name, ShortName, ParentCode, CategoryCode, ContractorId, CreateOn, ModifiedOn, Description, IsEnabled, SortCode);
        }

        protected override bool DoEquals(CatalogState other)
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
