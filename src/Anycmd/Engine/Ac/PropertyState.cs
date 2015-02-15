
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Exceptions;
    using Host;
    using Model;
    using Rdb;
    using System;
    using System.Reflection;
    using Util;

    /// <summary>
    /// 表示实体属性业务实体。
    /// </summary>
    public sealed class PropertyState : StateObject<PropertyState>, IProperty, IStateObject
    {
        private IAcDomain _acDomain;
        private Guid _entityTypeId;
        private Guid? _dicId;
        private Guid? _foreignPropertyId;
        private string _code;
        private string _name;
        private string _guideWords;
        private string _tooltip;
        private int? _maxLength;
        private int _sortCode;
        private string _icon;
        private bool _isDetailsShow;
        private bool _isDeveloperOnly;
        private bool _isInput;
        private string _inputType;
        private bool _isTotalLine;
        private DateTime? _createOn;
        private PropertyInfo _propertyInfo;
        private bool _propertyInfoed = false;
        private EntityTypeState _entityType = EntityTypeState.Empty;

        private PropertyState(Guid id) : base(id) { }

        #region 工厂方法
        public static PropertyState Create(IAcDomain acDomain, PropertyBase property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            if (property.EntityTypeId == Guid.Empty)
            {
                throw new AnycmdException("实体属性必须属于某个实体类型");
            }
            EntityTypeState entityType;
            if (!acDomain.EntityTypeSet.TryGetEntityType(property.EntityTypeId, out entityType))
            {
                throw new AnycmdException("意外的实体类型标识" + property.EntityTypeId);
            }
            Guid? dicId = property.DicId;
            if (dicId == Guid.Empty)
            {
                dicId = null;
            }
            if (dicId.HasValue)
            {
                if (!acDomain.DicSet.ContainsDic(dicId.Value))
                {
                    throw new ValidationException("意外的字典标识" + dicId);
                }
            }
            return new PropertyState(property.Id)
            {
                _acDomain = acDomain,
                _entityTypeId = property.EntityTypeId,
                _foreignPropertyId = property.ForeignPropertyId,
                _code = property.Code,
                _createOn = property.CreateOn,
                _dicId = dicId,
                _guideWords = property.GuideWords,
                _icon = property.Icon,
                _inputType = property.InputType,
                _isDetailsShow = property.IsDetailsShow,
                _maxLength = property.MaxLength,
                _name = property.Name,
                _sortCode = property.SortCode,
                _tooltip = property.Tooltip,
                _isTotalLine = property.IsTotalLine,
                _isDeveloperOnly = property.IsDeveloperOnly,
                _isInput = property.IsInput
            };
        }

        public static PropertyState CreateNoneProperty(string code)
        {
            return new PropertyState(Guid.Empty)
            {
                _acDomain = EmptyAcDomain.SingleInstance,
                _entityTypeId = EntityTypeState.Empty.Id,
                _foreignPropertyId = null,
                _code = code,
                _createOn = SystemTime.MinDate,
                _dicId = Guid.Empty,
                _guideWords = "警告：编码为" + code + "的字段不存在",
                _icon = string.Empty,
                _inputType = string.Empty,
                _isDetailsShow = false,
                _isInput = false,
                _maxLength = 0,
                _name = code,
                _sortCode = 0,
                _tooltip = "不存在的字段",
                _isTotalLine = false,
                _isDeveloperOnly = false
            };
        }
        #endregion

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public Guid EntityTypeId
        {
            get { return _entityTypeId; }
        }

        public Guid? ForeignPropertyId
        {
            get { return _foreignPropertyId; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string GuideWords
        {
            get { return _guideWords; }
        }

        public string Tooltip
        {
            get { return _tooltip; }
        }

        public int? MaxLength
        {
            get { return _maxLength; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public Guid? DicId
        {
            get { return _dicId; }
        }

        public string Icon
        {
            get { return _icon; }
        }

        public bool IsDetailsShow
        {
            get { return _isDetailsShow; }
        }

        public bool IsDeveloperOnly
        {
            get { return _isDeveloperOnly; }
        }

        public bool IsInput
        {
            get { return _isInput; }
        }

        public string InputType
        {
            get { return _inputType; }
        }

        public bool IsTotalLine
        {
            get { return _isTotalLine; }
        }

        public bool IsViewField
        {
            get { return PropertyInfo == null; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public PropertyInfo PropertyInfo
        {
            get
            {
                if (_propertyInfoed)
                {
                    return _propertyInfo;
                }
                _propertyInfo = this.EntityType.Map.ClrType.GetProperty(this.Code);
                _propertyInfoed = true;
                return _propertyInfo;
            }
        }

        private EntityTypeState EntityType
        {
            get
            {
                if (_entityType == EntityTypeState.Empty)
                {
                    if (!AcDomain.EntityTypeSet.TryGetEntityType(this.EntityTypeId, out _entityType))
                    {
                        throw new AnycmdException("意外的实体类型标识" + this.EntityTypeId);
                    }
                }
                return _entityType;
            }
        }

        private RdbDescriptor _database;
        private RdbDescriptor Database
        {
            get
            {
                if (_database == null)
                {
                    if (!AcDomain.Rdbs.TryDb(EntityType.DatabaseId, out _database))
                    {
                        throw new AnycmdException("意外的数据库标识" + EntityType.DatabaseId);
                    }
                }
                return _database;
            }
        }

        private DbTableColumn _tableColumn;
        private DbTableColumn TableColumn
        {
            get
            {
                if (_tableColumn == null)
                {
                    if (string.IsNullOrEmpty(EntityType.TableName))
                    {
                        return null;
                    }
                    AcDomain.Rdbs.DbTableColumns.TryGetDbTableColumn(Database, string.Format("[{0}][{1}][{2}]", EntityType.SchemaName, EntityType.TableName, this.Code), out _tableColumn);
                }

                return _tableColumn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConfigValid
        {
            get
            {
                if (this.IsViewField)
                {
                    return true;
                }
                bool isValid = true;
                if (PropertyInfo != null && !PropertyInfo.Name.Equals(this.Code, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(EntityType.TableName))
                {
                    if (TableColumn == null)
                    {
                        isValid = false;
                    }
                    else if (TableColumn.MaxLength.HasValue && TableColumn.MaxLength > 0 && this.MaxLength > TableColumn.MaxLength)
                    {
                        isValid = false;
                    }
                }

                return isValid;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DbIsNullable
        {
            get
            {
                return TableColumn != null && TableColumn.IsNullable;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DbTypeName
        {
            get
            {
                return TableColumn == null ? string.Empty : TableColumn.TypeName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DbMaxLength
        {
            get
            {
                return TableColumn == null ? null : TableColumn.MaxLength;
            }
        }

        protected override bool DoEquals(PropertyState other)
        {
            return Id == other.Id &&
                EntityTypeId == other.EntityTypeId &&
                ForeignPropertyId == other.ForeignPropertyId &&
                Code == other.Code &&
                Name == other.Name &&
                GuideWords == other.GuideWords &&
                Tooltip == other.Tooltip &&
                MaxLength == other.MaxLength &&
                PropertyInfo == other.PropertyInfo &&
                SortCode == other.SortCode &&
                DicId == other.DicId &&
                Icon == other.Icon &&
                IsDetailsShow == other.IsDetailsShow &&
                IsDeveloperOnly == other.IsDeveloperOnly &&
                InputType == other.InputType &&
                IsInput == other.IsInput &&
                IsTotalLine == other.IsTotalLine &&
                IsViewField == other.IsViewField;
        }
    }
}
