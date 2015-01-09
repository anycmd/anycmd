
namespace Anycmd.Ac.ViewModels
{
    using Engine;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Exceptions;
    using GroupViewModels;
    using Infra.AppSystemViewModels;
    using Infra.ButtonViewModels;
    using Infra.DicViewModels;
    using Infra.EntityTypeViewModels;
    using Infra.FunctionViewModels;
    using Infra.ResourceViewModels;
    using Infra.UIViewViewModels;
    using PrivilegeViewModels;
    using Rdb;
    using RdbViewModels;
    using RoleViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;
    using ViewModel;

    public static class AcDomainExtension
    {
        private static EntityTypeState GetEntityType(this IAcDomain host, Coder code)
        {
            EntityTypeState entityTypeState;
            if (!host.EntityTypeSet.TryGetEntityType(code, out entityTypeState))
            {
                throw new InvalidEntityTypeCodeException(code);
            }
            return entityTypeState;
        }

        #region GetPlistAppSystems
        public static IQueryable<AppSystemTr> GetPlistAppSystems(this IAcDomain host, GetPlistResult requestData)
        {
            EntityTypeState appSystemEntityType = host.GetEntityType(new Coder("Ac", "AppSystem"));
            foreach (var filter in requestData.Filters)
            {
                PropertyState property;
                if (!appSystemEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的AppSystem实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = host.AppSystemSet.Select(a => AppSystemTr.Create(host, a)).AsQueryable();
            foreach (var filter in requestData.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistButtons
        public static IQueryable<ButtonTr> GetPlistButtons(this IAcDomain host, GetPlistResult requestData)
        {
            EntityTypeState buttonEntityType = host.GetEntityType(new Coder("Ac", "Button"));
            foreach (var filter in requestData.Filters)
            {
                PropertyState property;
                if (!buttonEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Button实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = host.ButtonSet.Select(ButtonTr.Create).AsQueryable();
            foreach (var filter in requestData.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistUIViewButtons
        public static IQueryable<UiViewAssignButtonTr> GetPlistUiViewButtons(this IAcDomain host, GetPlistUiViewButtons requestData)
        {
            if (!requestData.UiViewId.HasValue)
            {
                throw new ValidationException("uiViewID是必须的");
            }
            UiViewState view;
            if (!host.UiViewSet.TryGetUiView(requestData.UiViewId.Value, out view))
            {
                throw new ValidationException("意外的页面标识" + requestData.UiViewId);
            }
            var viewButtons = host.UiViewSet.GetUiViewButtons(view);
            var buttons = host.ButtonSet;
            var data = new List<UiViewAssignButtonTr>();
            foreach (var button in buttons)
            {
                var viewButton = viewButtons.FirstOrDefault(a => a.ButtonId == button.Id && a.UiViewId == view.Id);
                if (requestData.IsAssigned.HasValue)
                {
                    if (requestData.IsAssigned.Value)
                    {
                        if (viewButton == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (viewButton != null)
                        {
                            continue;
                        }
                    }
                }
                DateTime? createOn = null;
                int isEnabled;
                string functionCode = null;
                Guid? functionId = null;
                string functionName = null;
                Guid id;
                bool isAssigned;
                int functionIsEnabled = 0;
                if (viewButton != null)
                {
                    createOn = viewButton.CreateOn;
                    isEnabled = viewButton.IsEnabled;
                    id = viewButton.Id;
                    isAssigned = true;
                    FunctionState function;
                    if (viewButton.FunctionId.HasValue && host.FunctionSet.TryGetFunction(viewButton.FunctionId.Value, out function))
                    {
                        functionCode = function.Code;
                        functionId = function.Id;
                        functionName = function.Description;
                        functionIsEnabled = function.IsEnabled;
                    }
                }
                else
                {
                    id = Guid.NewGuid();
                    isAssigned = false;
                    isEnabled = 0;
                }
                data.Add(new UiViewAssignButtonTr
                {
                    CreateOn = createOn,
                    Id = id,
                    IsAssigned = isAssigned,
                    FunctionIsEnabled = functionIsEnabled,
                    Icon = button.Icon,
                    IsEnabled = isEnabled,
                    Name = button.Name,
                    ButtonId = button.Id,
                    ButtonIsEnabled = button.IsEnabled,
                    Code = button.Code,
                    FunctionCode = functionCode,
                    FunctionId = functionId,
                    FunctionName = functionName,
                    UiViewId = view.Id,
                    SortCode = button.SortCode
                });
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.key))
            {
                queryable = queryable.Where(a => a.Name.Contains(requestData.key) || a.Code.Contains(requestData.key));
            }
            requestData.Total = queryable.Count();
            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistDatabases
        public static IQueryable<IRDatabase> GetPlistDatabases(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState entityType = host.GetEntityType(new Coder("Ac", "RDatabase"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!entityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的RDatabase实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.Rdbs.Select(a => a.Database).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistTables
        public static IQueryable<DbTable> GetPlistTables(this IAcDomain host, GetPlistTables requestModel)
        {
            if (!requestModel.DatabaseId.HasValue)
            {
                throw new ValidationException("databaseId为空");
            }
            RdbDescriptor db;
            if (!host.Rdbs.TryDb(requestModel.DatabaseId.Value, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            var properties = new HashSet<string>()
            {
                "Id",
                "DatabaseId",
                "CatalogName",
                "SchemaName",
                "Name",
                "Description"
            };
            foreach (var filter in requestModel.Filters)
            {
                if (!properties.Contains(filter.field))
                {
                    throw new ValidationException("意外的DbTable实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = db.DbTables.Values.Select(a => a).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistViews
        public static IQueryable<DbView> GetPlistViews(this IAcDomain host, GetPlistViews requestModel)
        {
            if (!requestModel.DatabaseId.HasValue)
            {
                throw new ValidationException("databaseId为空");
            }
            RdbDescriptor db;
            if (!host.Rdbs.TryDb(requestModel.DatabaseId.Value, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            var properties = new HashSet<string>()
            {
                "Id",
                "DatabaseId",
                "CatalogName",
                "SchemaName",
                "Name",
                "Description"
            };
            foreach (var filter in requestModel.Filters)
            {
                if (!properties.Contains(filter.field))
                {
                    throw new ValidationException("意外的DbView实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = db.DbViews.Values.Select(a => a).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistTableColumns
        public static IQueryable<DbTableColumn> GetPlistTableColumns(this IAcDomain host, GetPlistTableColumns requestModel)
        {
            if (!requestModel.DatabaseId.HasValue)
            {
                throw new ValidationException("databaseId为空");
            }
            RdbDescriptor db;
            if (!host.Rdbs.TryDb(requestModel.DatabaseId.Value, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbTable dbTable;
            if (!db.TryGetDbTable(requestModel.TableId, out dbTable))
            {
                throw new ValidationException("意外的数据库表名" + requestModel.TableName);
            }
            var properties = new HashSet<string>()
            {
                "DatabaseId",
                "Id",
                "CatalogName",
                "DateTimePrecision",
                "DefaultValue",
                "Description",
                "IsIdentity",
                "IsNullable",
                "IsPrimaryKey",
                "IsStoreGenerated",
                "MaxLength",
                "Name",
                "Ordinal",
                "Precision",
                "Scale",
                "SchemaName",
                "TableName",
                "TypeName"
            };
            foreach (var filter in requestModel.Filters)
            {
                if (!properties.Contains(filter.field))
                {
                    throw new ValidationException("意外的DbTableColumn实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            IReadOnlyDictionary<string, DbTableColumn> dbTableColumns;
            if (!host.Rdbs.DbTableColumns.TryGetDbTableColumns(db, dbTable, out dbTableColumns))
            {
                throw new AnycmdException("意外的数据库表列");
            }
            var queryable = dbTableColumns.Values.Select(a => a).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistViewColumns
        public static IQueryable<DbViewColumn> GetPlistViewColumns(this IAcDomain host, GetPlistViewColumns requestModel)
        {
            if (!requestModel.DatabaseId.HasValue)
            {
                throw new ValidationException("databaseId为空");
            }
            RdbDescriptor db;
            if (!host.Rdbs.TryDb(requestModel.DatabaseId.Value, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbView dbView;
            if (!db.TryGetDbView(requestModel.ViewId, out dbView))
            {
                throw new ValidationException("意外的数据库表名" + requestModel.ViewName);
            } 
            var properties = new HashSet<string>()
            {
                "DatabaseId",
                "Id",
                "CatalogName",
                "DateTimePrecision",
                "DefaultValue",
                "Description",
                "IsIdentity",
                "IsNullable",
                "IsPrimaryKey",
                "IsStoreGenerated",
                "MaxLength",
                "Name",
                "Ordinal",
                "Precision",
                "Scale",
                "SchemaName",
                "TableName",
                "TypeName"
            };
            foreach (var filter in requestModel.Filters)
            {
                if (!properties.Contains(filter.field))
                {
                    throw new ValidationException("意外的DbViewColumn实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            IReadOnlyDictionary<string, DbViewColumn> dbViewColumns;
            if (!host.Rdbs.DbViewColumns.TryGetDbViewColumns(db, dbView, out dbViewColumns))
            {
                throw new AnycmdException("意外的数据库视图列");
            }
            var queryable = dbViewColumns.Values.Select(a => a).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistDics
        public static IQueryable<DicTr> GetPlistDics(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState dicEntityType = host.GetEntityType(new Coder("Ac", "Dic"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!dicEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Dic实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.DicSet.Select(DicTr.Create).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistDicItems
        public static IQueryable<DicItemTr> GetPlistDicItems(this IAcDomain host, GetPlistDicItems requestModel)
        {
            if (!requestModel.DicId.HasValue)
            {
                throw new ValidationException("字典标识是必须的");
            }
            if (!host.DicSet.ContainsDic(requestModel.DicId.Value))
            {
                throw new ValidationException("意外的系统字典标识" + requestModel.DicId);
            }
            EntityTypeState dicItemEntityType = host.GetEntityType(new Coder("Ac", "DicItem"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!dicItemEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的DicItem实体类型属性" + filter.field);
                }
            }
            DicState dic;
            if (!host.DicSet.TryGetDic(requestModel.DicId.Value, out dic))
            {
                throw new ValidationException("意外的字典标识" + requestModel.DicId.Value);
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.DicSet.GetDicItems(dic).Select(a => DicItemTr.Create(a.Value)).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            if (!string.IsNullOrEmpty(requestModel.Key))
            {
                queryable = queryable.Where(a => a.Code.ToLower().Contains(requestModel.Key) || a.Name.ToLower().Contains(requestModel.Key));
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistEntityTypes
        public static IQueryable<EntityTypeTr> GetPlistEntityTypes(this IAcDomain host, GetPlistResult requestData)
        {
            EntityTypeState entityTypeEntityType = host.GetEntityType(new Coder("Ac", "EntityType"));
            foreach (var filter in requestData.Filters)
            {
                PropertyState property;
                if (!host.EntityTypeSet.TryGetProperty(entityTypeEntityType, filter.field, out property))
                {
                    throw new ValidationException("意外的EntityType实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = host.EntityTypeSet.Select(EntityTypeTr.Create).AsQueryable();
            foreach (var filter in requestData.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistFunctions
        public static IQueryable<FunctionTr> GetPlistFunctions(this IAcDomain host, GetPlistResult requestData)
        {
            EntityTypeState entityType = host.GetEntityType(new Coder("Ac", "Function"));
            foreach (var filter in requestData.Filters)
            {
                PropertyState property;
                if (!host.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Function实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = host.FunctionSet.Select(FunctionTr.Create).AsQueryable();
            foreach (var filter in requestData.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistPrivilegeByRoleId
        public static IQueryable<RoleAssignFunctionTr> GetPlistPrivilegeByRoleId(this IAcDomain host, GetPlistFunctionByRoleId requestData)
        {
            AppSystemState appSystem;
            if (!host.AppSystemSet.TryGetAppSystem(requestData.AppSystemId, out appSystem))
            {
                throw new ValidationException("意外的应用系统标识" + requestData.AppSystemId);
            }
            RoleState role;
            if (!host.RoleSet.TryGetRole(requestData.RoleId, out role))
            {
                throw new ValidationException("意外的角色标识" + requestData.RoleId);
            }
            if (requestData.ResourceTypeId.HasValue)
            {
                ResourceTypeState resource;
                if (!host.ResourceTypeSet.TryGetResource(requestData.ResourceTypeId.Value, out resource))
                {
                    throw new ValidationException("意外的资源标识" + requestData.ResourceTypeId);
                }
            }
            var roleFunctions = host.PrivilegeSet.Where(a => a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Function).ToList();
            var functions = host.FunctionSet.Where(a => a.AppSystem.Id == requestData.AppSystemId && a.IsManaged);
            if (requestData.ResourceTypeId.HasValue)
            {
                functions = functions.Where(a => a.ResourceTypeId == requestData.ResourceTypeId.Value);
            }
            var data = new List<RoleAssignFunctionTr>();
            foreach (var function in functions)
            {
                var roleFunction = roleFunctions.FirstOrDefault(a => a.SubjectInstanceId == role.Id && a.ObjectInstanceId == function.Id);
                if (requestData.IsAssigned.HasValue)
                {
                    if (requestData.IsAssigned.Value)
                    {
                        if (roleFunction == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (roleFunction != null)
                        {
                            continue;
                        }
                    }
                }
                string createBy = null;
                DateTime? createOn = null;
                Guid? createUserId = null;
                Guid id;
                bool isAssigned;
                string orientation = null;
                string privilegeConstraint = null;
                if (roleFunction != null)
                {
                    createBy = roleFunction.CreateBy;
                    createOn = roleFunction.CreateOn;
                    createUserId = roleFunction.CreateUserId;
                    id = roleFunction.Id;
                    isAssigned = true;
                    orientation = roleFunction.AcContentType;
                    privilegeConstraint = roleFunction.AcContent;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAssigned = false;
                }
                data.Add(new RoleAssignFunctionTr
                {
                    AppSystemCode = appSystem.Code,
                    AppSystemId = appSystem.Id,
                    CreateBy = createBy,
                    CreateOn = createOn,
                    CreateUserId = createUserId,
                    Description = function.Description,
                    FunctionCode = function.Code,
                    FunctionId = function.Id,
                    Id = id,
                    IsAssigned = isAssigned,
                    AcContentType = orientation,
                    AcContent = privilegeConstraint,
                    ResourceCode = function.Resource.Code,
                    ResourceTypeId = function.ResourceTypeId,
                    ResourceName = function.Resource.Name,
                    RoleId = requestData.RoleId,
                    SortCode = function.SortCode
                });
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a.Description.Contains(requestData.Key) || a.FunctionCode.Contains(requestData.Key));
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistGroups
        public static IQueryable<IGroup> GetPlistGroups(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState groupEntityType = host.GetEntityType(new Coder("Ac", "Group"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!groupEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Group实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.GroupSet.AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistRoleGroups
        public static IQueryable<Dictionary<string, object>> GetPlistRoleGroups(this IAcDomain host, GetPlistRoleGroups requestData)
        {
            RoleState role;
            if (!host.RoleSet.TryGetRole(requestData.RoleId, out role))
            {
                throw new ValidationException("意外的角色标识" + requestData.RoleId);
            }
            var data = new List<Dictionary<string, object>>();
            foreach (var group in host.GroupSet)
            {
                var roleGroup = host.PrivilegeSet.FirstOrDefault(a =>
                    a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Group
                    && a.SubjectInstanceId == role.Id && a.ObjectInstanceId == group.Id);
                if (requestData.IsAssigned.HasValue)
                {
                    if (requestData.IsAssigned.Value)
                    {
                        if (roleGroup == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (roleGroup != null)
                        {
                            continue;
                        }
                    }
                }
                string createBy = null;
                DateTime? createOn = null;
                Guid? createUserId = null;
                Guid id;
                bool isAssigned;
                if (roleGroup != null)
                {
                    createBy = roleGroup.CreateBy;
                    createOn = roleGroup.CreateOn;
                    createUserId = roleGroup.CreateUserId;
                    id = roleGroup.Id;
                    isAssigned = true;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAssigned = false;
                }
                data.Add(new Dictionary<string, object>
                {
                    {"CategoryCode", group.CategoryCode??string.Empty},
                    {"CreateBy", createBy},
                    {"CreateOn", createOn},
                    {"CreateUserId", createUserId},
                    {"GroupId", group.Id},
                    {"Id", id},
                    {"IsAssigned", isAssigned},
                    {"IsEnabled", group.IsEnabled},
                    {"Name", group.Name??string.Empty},
                    {"RoleId", role.Id},
                    {"SortCode", group.SortCode}
                });
            }
            var pageIndex = requestData.PageIndex;
            var pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a["Name"].ToString().Contains(requestData.Key) || a["CategoryCode"].ToString().Contains(requestData.Key));
            }
            requestData.Total = queryable.Count();
            if (requestData.Total > 0)
            {
                var firstRecord = queryable.First();
                if (!firstRecord.Keys.Contains(requestData.SortField))
                {
                    throw new ValidationException("意外的字段" + requestData.SortField);
                }
                var sortOrder = (requestData.SortOrder ?? "asc").ToLower();
                switch (sortOrder)
                {
                    case "asc":
                        queryable = queryable.OrderBy(a => a[requestData.SortField]);
                        break;
                    case "desc":
                        queryable = queryable.OrderByDescending(a => a[requestData.SortField]);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            return queryable.Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistUiViews
        public static IQueryable<UiViewTr> GetPlistUiViews(this IAcDomain host, GetPlistResult requestData)
        {
            EntityTypeState viewEntityType = host.GetEntityType(new Coder("Ac", "UIView"));
            foreach (var filter in requestData.Filters)
            {
                PropertyState property;
                if (!viewEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的UIView实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = host.UiViewSet.Select(UiViewTr.Create).AsQueryable();
            foreach (var filter in requestData.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistProperties
        public static IQueryable<PropertyTr> GetPlistProperties(this IAcDomain host, GetPlistProperties requestModel)
        {
            if (!requestModel.EntityTypeId.HasValue)
            {
                throw new ValidationException("entityTypeID是必须的");
            }
            EntityTypeState entityType = host.GetEntityType(new Coder("Ac", "Property"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!host.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Property实体类型属性" + filter.field);
                }
            }
            if (!host.EntityTypeSet.TryGetEntityType(requestModel.EntityTypeId.Value, out entityType))
            {
                throw new AnycmdException("意外的实体类型标识" + requestModel.EntityTypeId.Value);
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.EntityTypeSet.GetProperties(entityType).Select(a => PropertyTr.Create(a.Value)).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            if (!string.IsNullOrEmpty(requestModel.Key))
            {
                queryable = queryable.Where(a => a.Code.ToLower().Contains(requestModel.Key) || a.Name.ToLower().Contains(requestModel.Key));
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistResources
        public static IQueryable<ResourceTypeTr> GetPlistResources(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState entityType = host.GetEntityType(new Coder("Ac", "ResourceType"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!entityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Resource实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.ResourceTypeSet.Select(ResourceTypeTr.Create).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistAppSystemResources
        public static IQueryable<ResourceTypeTr> GetPlistAppSystemResources(this IAcDomain host, GetPlistAreaResourceTypes requestModel)
        {
            if (!host.AppSystemSet.ContainsAppSystem(requestModel.AppSystemId))
            {
                throw new AnycmdException("意外的应用系统标识" + requestModel.AppSystemId);
            }
            IEnumerable<Guid> resourceTypeIDs = host.FunctionSet.Where(a => a.AppSystem.Id == requestModel.AppSystemId).Select(a => a.ResourceTypeId).Distinct();
            var resources = new List<ResourceTypeState>();
            foreach (var resourceTypeId in resourceTypeIDs)
            {
                ResourceTypeState resource;
                host.ResourceTypeSet.TryGetResource(resourceTypeId, out resource);
                resources.Add(resource);
            }
            EntityTypeState entityType = host.GetEntityType(new Coder("Ac", "ResourceType"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!entityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Resource实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = resources.Select(ResourceTypeTr.Create).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            if (!string.IsNullOrEmpty(requestModel.Key))
            {
                queryable = queryable.Where(a => a.Code.ToLower().Contains(requestModel.Key) || a.Name.ToLower().Contains(requestModel.Key));
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistRoles
        public static IQueryable<IRole> GetPlistRoles(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState roleEntityType = host.GetEntityType(new Coder("Ac", "Role"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!roleEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Role实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.RoleSet.AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistSsdSets
        public static IQueryable<ISsdSet> GetPlistSsdSets(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState roleEntityType = host.GetEntityType(new Coder("Ac", "SsdSet"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!roleEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的SsdSet实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.SsdSetSet.AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistDsdSets
        public static IQueryable<IDsdSet> GetPlistDsdSets(this IAcDomain host, GetPlistResult requestModel)
        {
            EntityTypeState roleEntityType = host.GetEntityType(new Coder("Ac", "DsdSet"));
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!roleEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的DsdSet实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = host.DsdSetSet.AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestModel.Total = queryable.Count();

            return queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistGroupRoles
        public static IQueryable<GroupAssignRoleTr> GetPlistGroupRoles(this IAcDomain host, GetPlistGroupRoles requestData)
        {
            GroupState group;
            if (!host.GroupSet.TryGetGroup(requestData.GroupId, out group))
            {
                throw new ValidationException("意外的工作组标识" + requestData.GroupId);
            }
            var data = new List<GroupAssignRoleTr>();
            foreach (var role in host.RoleSet)
            {
                var roleGroup = host.PrivilegeSet.FirstOrDefault(a =>
                    a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Group 
                    && a.SubjectInstanceId == role.Id && a.ObjectInstanceId == group.Id);
                if (requestData.IsAssigned.HasValue)
                {
                    if (requestData.IsAssigned.Value)
                    {
                        if (roleGroup == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (roleGroup != null)
                        {
                            continue;
                        }
                    }
                }
                string createBy = null;
                DateTime? createOn = null;
                Guid? createUserId = null;
                Guid id;
                bool isAssigned;
                if (roleGroup != null)
                {
                    createBy = roleGroup.CreateBy;
                    createOn = roleGroup.CreateOn;
                    createUserId = roleGroup.CreateUserId;
                    id = roleGroup.Id;
                    isAssigned = true;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAssigned = false;
                }
                data.Add(new GroupAssignRoleTr
                {
                    CategoryCode = role.CategoryCode,
                    CreateBy = createBy,
                    CreateOn = createOn,
                    CreateUserId = createUserId,
                    GroupId = group.Id,
                    Id = id,
                    IsAssigned = isAssigned,
                    IsEnabled = role.IsEnabled,
                    Name = role.Name,
                    RoleId = role.Id,
                    SortCode = role.SortCode,
                    Icon = role.Icon
                });
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a.Name.Contains(requestData.Key) || a.CategoryCode.Contains(requestData.Key));
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region GetPlistMenuRoles
        public static IQueryable<MenuAssignRoleTr> GetPlistMenuRoles(this IAcDomain host, GetPlistMenuRoles requestData)
        {
            if (!requestData.MenuId.HasValue)
            {
                throw new ValidationException("menuID为空");
            }
            MenuState menu;
            if (!host.MenuSet.TryGetMenu(requestData.MenuId.Value, out menu))
            {
                throw new ValidationException("意外的菜单标识" + requestData.MenuId);
            }
            var roleMenus = host.PrivilegeSet.Where(a => a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Menu).ToList();
            var roles = host.RoleSet;
            var data = new List<MenuAssignRoleTr>();
            foreach (var role in roles)
            {
                var roleMenu = roleMenus.FirstOrDefault(a => a.SubjectInstanceId == role.Id && a.ObjectInstanceId == menu.Id);
                if (requestData.IsAssigned.HasValue)
                {
                    if (requestData.IsAssigned.Value)
                    {
                        if (roleMenu == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (roleMenu != null)
                        {
                            continue;
                        }
                    }
                }
                string createBy = null;
                DateTime? createOn = null;
                Guid? createUserId = null;
                Guid id;
                bool isAssigned;
                string orientation = null;
                string privilegeConstraint = null;
                if (roleMenu != null)
                {
                    createBy = roleMenu.CreateBy;
                    createOn = roleMenu.CreateOn;
                    createUserId = roleMenu.CreateUserId;
                    id = roleMenu.Id;
                    isAssigned = true;
                    orientation = roleMenu.AcContentType;
                    privilegeConstraint = roleMenu.AcContent;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAssigned = false;
                }
                data.Add(new MenuAssignRoleTr
                {
                    CreateBy = createBy,
                    CreateOn = createOn,
                    CreateUserId = createUserId,
                    Id = id,
                    IsAssigned = isAssigned,
                    AcContentType = orientation,
                    AcContent = privilegeConstraint,
                    RoleId = role.Id,
                    CategoryCode = role.CategoryCode,
                    Icon = role.Icon,
                    IsEnabled = role.IsEnabled,
                    MenuId = requestData.MenuId.Value,
                    Name = role.Name,
                    SortCode = role.SortCode
                });
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a.Name.Contains(requestData.Key) || a.CategoryCode.Contains(requestData.Key));
            }
            requestData.Total = queryable.Count();

            return queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion
    }
}
