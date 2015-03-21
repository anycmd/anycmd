
namespace Anycmd.Engine.Ac
{
    using Exceptions;
    using Functions;
    using Host;
    using Model;
    using Privileges;
    using System;
    using Util;

    /// <summary>
    /// 表示标识过程的业务实体。
    /// <remarks>
    /// Operation = user oriented Function;Permission = managed Function;Function = Action + ResourceType。
    /// </remarks>
    /// </summary>
    public sealed class FunctionState : StateObject<FunctionState>, IFunction, IAcElement
    {
        private Guid _resourceTypeId;
        private IAcDomain _acDomain;
        private Guid? _guid;
        private string _code;
        private bool _isManaged;
        private int _isEnabled;
        private Guid _developerId;
        private int _sortCode;
        private string _description;
        private DateTime? _createOn;

        public static readonly FunctionState Empty = new FunctionState(System.Guid.Empty)
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _code = string.Empty,
            _guid = System.Guid.Empty,
            _createOn = SystemTime.MinDate,
            _description = string.Empty,
            _developerId = System.Guid.Empty,
            _resourceTypeId = System.Guid.Empty,
            _isEnabled = 0,
            _isManaged = false,
            _sortCode = 0
        };

        private FunctionState(Guid id) : base(id) { }

        public static FunctionState Create(IAcDomain acDomain, FunctionBase function)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }
            return new FunctionState(function.Id)
            {
                _acDomain = acDomain,
                _createOn = function.CreateOn
            }.InternalModify(function);
        }

        internal FunctionState InternalModify(FunctionBase function)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }
            if (function.ResourceTypeId == System.Guid.Empty)
            {
                throw new AnycmdException("必须指定资源");
            }
            CatalogState resource;
            if (!_acDomain.CatalogSet.TryGetCatalog(function.ResourceTypeId, out resource))
            {
                throw new ValidationException("非法的资源标识" + function.ResourceTypeId);
            }
            // ResourceTypeId是来自于Catalog树上的节点，ResourceTypeId是可以改变的。
            _resourceTypeId = function.ResourceTypeId;
            _code = function.Code;
            _guid = function.Guid;
            _isManaged = function.IsManaged;
            _isEnabled = function.IsEnabled;
            _developerId = function.DeveloperId;
            _sortCode = function.SortCode;
            _description = function.Description;

            return this;
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Function; }
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public AppSystemState AppSystem
        {
            get
            {
                if (this == Empty)
                {
                    return AppSystemState.Empty;
                }
                AppSystemState appSystem;
                if (!AcDomain.AppSystemSet.TryGetAppSystem(this.Resource.Code.Substring(0, this.Resource.Code.IndexOf('.')), out appSystem))
                {
                    throw new AnycmdException("意外的应用系统标识");
                }
                return appSystem;
            }
        }

        public CatalogState Resource
        {
            get
            {
                if (this == Empty)
                {
                    return CatalogState.Empty;
                }
                CatalogState resource;
                if (!AcDomain.CatalogSet.TryGetCatalog(this.ResourceTypeId, out resource))
                {
                    throw new AnycmdException("意外的资源标识");
                }
                return resource;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ResourceTypeId
        {
            get { return _resourceTypeId; }
        }

        public Guid? Guid
        {
            get { return _guid; }
        }

        public string Code
        {
            get { return _code; }
        }

        public bool IsManaged
        {
            get { return _isManaged; }
        }

        public bool IsOperation
        {
            get { return true; }
        }

        /// <summary>
        /// 与IsManaged等值。
        /// </summary>
        public bool IsPermission
        {
            get { return _isManaged; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public Guid DeveloperId
        {
            get { return _developerId; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public string Description
        {
            get { return _description; }
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
    ResourceTypeId:'{1}',
    Guid:'{2}',
    Code:'{3}',
    IsManaged:{4},
    IsOperation:{5},
    IsPermission:{6},
    IsEnabled:{7},
    DeveloperId:'{8}',
    SortCode:{9},
    Description:'{10}',
    CreateOn:'{11}'
}}", Id, ResourceTypeId, Guid, Code, IsManaged, IsOperation, IsPermission, IsEnabled, DeveloperId, SortCode, Description, CreateOn);
        }

        protected override bool DoEquals(FunctionState other)
        {
            return Id == other.Id &&
                ResourceTypeId == other.ResourceTypeId &&
                Code == other.Code &&
                IsManaged == other.IsManaged &&
                IsEnabled == other.IsEnabled &&
                DeveloperId == other.DeveloperId &&
                SortCode == other.SortCode &&
                Description == other.Description;
        }
    }
}
