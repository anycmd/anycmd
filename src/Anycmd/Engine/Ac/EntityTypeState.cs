
namespace Anycmd.Engine.Ac
{
    using EntityTypes;
    using Exceptions;
    using Host;
    using Host.Ac.Infra;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 表示实体类型业务实体。
    /// </summary>
    public sealed class EntityTypeState : StateObject<EntityTypeState>, IEntityType
    {
        public const string DefaultCodespace = "Default";

        public static readonly EntityTypeState Empty = new EntityTypeState(Guid.Empty)
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _codespace = string.Empty,
            _code = string.Empty,
            _isCatalogued = false,
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
        private bool _isCatalogued;
        private Guid _databaseId;
        private Guid _developerId;
        private string _schemaName;
        private string _tableName;
        private int _sortCode;
        private int _editWidth;
        private int _editHeight;
        private DateTime? _createOn;

        private EntityTypeState(Guid id) : base(id) { }

        public static EntityTypeState Create(IAcDomain acDomain, EntityTypeBase entityType, EntityTypeMap map)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            return new EntityTypeState(entityType.Id)
            {
                _acDomain = acDomain,
                _map = map,
                _createOn = entityType.CreateOn,
            }.InternalModify(entityType);
        }

        internal EntityTypeState InternalModify(EntityTypeBase entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            if (!_acDomain.Rdbs.ContainsDb(entityType.DatabaseId))
            {
                throw new AnycmdException("意外的数据库" + entityType.DatabaseId);
            }
            _codespace = entityType.Codespace;
            _code = entityType.Code;
            _isCatalogued = entityType.IsCatalogued;
            _databaseId = entityType.DatabaseId;
            _developerId = entityType.DeveloperId;
            _editHeight = entityType.EditHeight;
            _editWidth = entityType.EditWidth;
            _name = entityType.Name;
            _schemaName = entityType.SchemaName;
            _sortCode = entityType.SortCode;
            _tableName = entityType.TableName;

            return this;
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public EntityTypeMap Map
        {
            get
            {
                return _map ?? EntityTypeMap.Empty;
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

        public bool IsCatalogued
        {
            get { return _isCatalogued; }
        }

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

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Codespace:'{1}',
    Code:'{2}',
    Name:'{3}',
    IsCatalogued:{4},
    DatabaseId:'{5}',
    DeveloperId:'{6}',
    SchemaName:'{7}',
    TableName:'{8}',
    SortCode:{9},
    EditWidth:{10},
    EditHeight:{11},
    CreateOn:'{12}'
}}", Id, Codespace, Code, Name, IsCatalogued, DatabaseId, DeveloperId, SchemaName, TableName, SortCode, EditWidth, EditHeight, CreateOn);
        }

        protected override bool DoEquals(EntityTypeState other)
        {
            return Id == other.Id &&
                Codespace == other.Codespace &&
                Code == other.Code &&
                Name == other.Name &&
                IsCatalogued == other.IsCatalogued &&
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
