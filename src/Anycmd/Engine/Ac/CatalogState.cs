
namespace Anycmd.Engine.Ac
{
    using Catalogs;
    using Host;
    using Model;
    using Privileges;
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
            _parentCode = null,
            _sortCode = 0
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
            _parentCode = null,
            _sortCode = 0
        };

        private IAcDomain _acDomain;
        private string _code;
        private string _name;
        private string _parentCode;
        private string _categoryCode;
        private DateTime? _createOn;
        private DateTime? _modifiedOn;
        private string _description;
        private int _isEnabled;
        private int _sortCode;

        private CatalogState(Guid id) : base(id) { }

        public static CatalogState Create(IAcDomain acDomain, CatalogBase catalog)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (catalog == null)
            {
                throw new ArgumentNullException("catalog");
            }
            return new CatalogState(catalog.Id)
            {
                _acDomain = acDomain,
                _createOn = catalog.CreateOn
            }.InternalModify(catalog);
        }

        internal CatalogState InternalModify(CatalogBase catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException("catalog");
            }
            _categoryCode = catalog.CategoryCode;
            _code = catalog.Code;
            _description = catalog.Description;
            _isEnabled = catalog.IsEnabled;
            _modifiedOn = catalog.ModifiedOn;
            _name = catalog.Name;
            _parentCode = catalog.ParentCode;
            _sortCode = catalog.SortCode;

            return this;
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

        public string ParentCode
        {
            get { return _parentCode; }
        }

        public string CategoryCode
        {
            get { return _categoryCode; }
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

        protected override bool DoEquals(CatalogState other)
        {
            return Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                ParentCode == other.ParentCode &&
                CategoryCode == other.CategoryCode &&
                IsEnabled == other.IsEnabled &&
                SortCode == other.SortCode &&
                Description == other.Description;
        }
    }
}
