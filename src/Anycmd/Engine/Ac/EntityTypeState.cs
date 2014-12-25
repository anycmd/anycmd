
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Exceptions;
    using Host;
    using Host.Ac.Infra;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 表示实体类型业务实体。
    /// </summary>
    public sealed class EntityTypeState : StateObject<EntityTypeState>, IEntityType, IStateObject
    {
        public static readonly EntityTypeState Empty = new EntityTypeState(Guid.Empty)
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _codespace = string.Empty,
            _code = string.Empty,
            _isOrganizational = false,
            _createOn = SystemTime.MinDate,
            _databaseId = Guid.Empty,
            _developerId = Guid.Empty,
            _editHeight = 0,
            _editWidth = 0,
            _name = string.Empty,
            _schemaName = string.Empty,
            _sortCode = 0,
            _tableName = string.Empty
        };

        private EntityTypeMap _map;
        private IAcDomain _acDomain;
        private string _codespace;
        private string _code;
        private string _name;
        private bool _isOrganizational;
        private Guid _databaseId;
        private Guid _developerId;
        private string _schemaName;
        private string _tableName;
        private int _sortCode;
        private int _editWidth;
        private int _editHeight;
        private DateTime? _createOn;

        private EntityTypeState(Guid id) : base(id) { }

        public static EntityTypeState Create(IAcDomain host, EntityTypeBase entityType, EntityTypeMap map)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            if (!host.Rdbs.ContainsDb(entityType.DatabaseId))
            {
                throw new CoreException("意外的数据库" + entityType.DatabaseId);
            }

            return new EntityTypeState(entityType.Id)
            {
                _acDomain = host,
                _map = map,
                _codespace = entityType.Codespace,
                _code = entityType.Code,
                _isOrganizational = entityType.IsOrganizational,
                _createOn = entityType.CreateOn,
                _databaseId = entityType.DatabaseId,
                _developerId = entityType.DeveloperId,
                _editHeight = entityType.EditHeight,
                _editWidth = entityType.EditWidth,
                _name = entityType.Name,
                _schemaName = entityType.SchemaName,
                _sortCode = entityType.SortCode,
                _tableName = entityType.TableName
            };
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public EntityTypeMap Map
        {
            get
            {
                if (_map == null)
                {
                    return EntityTypeMap.Empty;
                }
                return _map;
            }
        }

        public string Codespace
        {
            get { return _codespace; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsOrganizational
        {
            get { return _isOrganizational; }
        }

        // TODO:databaseID应该是可空的
        public Guid DatabaseId
        {
            get { return _databaseId; }
        }

        public Guid DeveloperId
        {
            get { return _developerId; }
        }

        public string SchemaName
        {
            get { return _schemaName; }
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public int EditWidth
        {
            get { return _editWidth; }
        }

        public int EditHeight
        {
            get { return _editHeight; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        protected override bool DoEquals(EntityTypeState other)
        {
            return Id == other.Id &&
                Codespace == other.Codespace &&
                Code == other.Code &&
                Name == other.Name &&
                IsOrganizational == other.IsOrganizational &&
                DatabaseId == other.DatabaseId &&
                SchemaName == other.SchemaName &&
                TableName == other.TableName &&
                SortCode == other.SortCode &&
                DeveloperId == other.DeveloperId &&
                EditHeight == other.EditHeight &&
                EditWidth == other.EditWidth;
        }
    }
}
