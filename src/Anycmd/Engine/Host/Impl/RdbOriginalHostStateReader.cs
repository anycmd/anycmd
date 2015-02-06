
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Identity;
    using Ac.Infra;
    using Ac.Rbac;
    using Engine.Rdb;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class RdbOriginalHostStateReader : IOriginalHostStateReader
    {
        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        private readonly string _bootConnString = ConfigurationManager.AppSettings["BootDbConnString"];

        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        public string BootConnString { get { return _bootConnString; } }

        private readonly IAcDomain _acDomain;
        private DataSet ds = null;
        private readonly List<string> _tableNames = new List<string>();

        public RdbOriginalHostStateReader(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        private DataTable this[string tableName]
        {
            get
            {
                if (ds != null) return ds.Tables[_tableNames.IndexOf(tableName)];
                lock (this)
                {
                    if (ds != null) return ds.Tables[_tableNames.IndexOf(tableName)];
                    ds = new DataSet();
                    using (var conn = new SqlConnection(this.BootConnString))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        var sb = new StringBuilder();
                        Append(sb, "AppSystem", "select * from [AppSystem];");
                        Append(sb, "Function", "select * from [Function];");
                        Append(sb, "Dic", "select * from [Dic];");
                        Append(sb, "DicItem", "select * from [DicItem];");
                        Append(sb, "EntityType", "select * from [EntityType];");
                        Append(sb, "Property", "select * from [Property];");
                        Append(sb, "Catalog", "select * from [Catalog];");
                        Append(sb, "Menu", "select * from [Menu];");
                        Append(sb, "Button", "select * from [Button];");
                        Append(sb, "Group", "select * from [Group] where TypeCode='Ac';");
                        Append(sb, "UiView", "select * from [UiView];");
                        Append(sb, "UiViewButton", "select * from [UiViewButton];");
                        Append(sb, "Privilege", "select * from [Privilege] where SubjectType!='Account';");// 查询非账户主体的权限记录，账户主体的权限记录在会话中查询
                        Append(sb, "Role", "select * from [Role];");
                        Append(sb, "SsdSet", "select * from [SsdSet];");
                        Append(sb, "DsdSet", "select * from [DsdSet];");
                        Append(sb, "SsdRole", "select * from [SsdRole];");
                        Append(sb, "DsdRole", "select * from [DsdRole];");
                        Append(sb, "RDatabase", "select * from [RDatabase] order by CatalogName;");
                        Append(sb, "Account", "select a.* from [Account] as a join DeveloperId as d on a.Id=d.Id;");
                        var cmd = conn.CreateCommand();
                        cmd.CommandText = sb.ToString();
                        cmd.CommandType = CommandType.Text;
                        var sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);
                    }
                }

                return ds.Tables[_tableNames.IndexOf(tableName)];
            }
        }

        private void Append(StringBuilder sb, string tableName, string sql)
        {
            sb.Append(sql);
            _tableNames.Add(tableName);
        }

        public IList<RDatabase> GetAllRDatabases()
        {
            return (from DataRow row in this["RDatabase"].Rows
                select new RDatabase
                {
                    CatalogName = (string) row["CatalogName"], 
                    Id = (Guid) row["Id"], 
                    IsTemplate = (bool) row["IsTemplate"], 
                    DataSource = (string) row["DataSource"], 
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(), 
                    Profile = row["Profile"] == DBNull.Value ? null : row["Profile"].ToString(), 
                    Password = (string) row["Password"], 
                    RdbmsType = (string) row["RdbmsType"], 
                    UserId = (string) row["UserId"], 
                    ProviderName = (string) row["ProviderName"]
                }).ToList();
        }

        public IList<DbTableColumn> GetTableColumns(RdbDescriptor db)
        {
            return _acDomain.RetrieveRequiredService<IRdbMetaDataService>().GetTableColumns(db);
        }

        public IList<DbTable> GetDbTables(RdbDescriptor db)
        {
            return _acDomain.RetrieveRequiredService<IRdbMetaDataService>().GetDbTables(db);
        }

        public IList<DbViewColumn> GetViewColumns(RdbDescriptor db)
        {
            return _acDomain.RetrieveRequiredService<IRdbMetaDataService>().GetViewColumns(db);
        }

        public IList<DbView> GetDbViews(RdbDescriptor db)
        {
            return _acDomain.RetrieveRequiredService<IRdbMetaDataService>().GetDbViews(db);
        }

        public IList<Catalog> GetCatalogs()
        {
            var list = new List<Catalog>();
            foreach (DataRow row in this["Catalog"].Rows)
            {
                var item = new Catalog
                {
                    Id = (Guid)row["Id"],
                    ParentCode = row["ParentCode"] == DBNull.Value ? null : row["ParentCode"].ToString(),
                    CategoryCode = row["CategoryCode"] == DBNull.Value ? null : row["CategoryCode"].ToString(),
                    Code = (string)row["Code"],
                    Name = (string)row["Name"],
                    DeletionStateCode = (int)row["DeletionStateCode"],
                    IsEnabled = (int)row["IsEnabled"],
                    SortCode = (int)row["SortCode"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<AppSystem> GetAllAppSystems()
        {
            var list = new List<AppSystem>();
            foreach (DataRow row in this["AppSystem"].Rows)
            {
                var item = new AppSystem
                {
                    Id = (Guid)row["Id"],
                    Code = (string)row["Code"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    ImageUrl = row["ImageUrl"] == DBNull.Value ? null : row["ImageUrl"].ToString(),
                    IsEnabled = (int)row["IsEnabled"],
                    Name = row["Name"] == DBNull.Value ? null : row["Name"].ToString(),
                    PrincipalId = (Guid)row["PrincipalId"],
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    SsoAuthAddress = row["SSOAuthAddress"] == DBNull.Value ? null : row["SSOAuthAddress"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Button> GetAllButtons()
        {
            var list = new List<Button>();
            foreach (DataRow row in this["Button"].Rows)
            {
                var item = new Button
                {
                    Id = (Guid)row["Id"],
                    CategoryCode = row["CategoryCode"] == DBNull.Value ? null : row["CategoryCode"].ToString(),
                    Code = (string)row["Code"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    IsEnabled = (int)row["IsEnabled"],
                    Name = (string)row["Name"],
                    SortCode = (int)row["SortCode"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];
                list.Add(item);
            }
            return list;
        }

        public IList<Dic> GetAllDics()
        {
            var list = new List<Dic>();
            foreach (DataRow row in this["Dic"].Rows)
            {
                var item = new Dic
                {
                    Id = (Guid)row["Id"],
                    Code = (string)row["Code"],
                    IsEnabled = (int)row["IsEnabled"],
                    Name = (string)row["Name"],
                    SortCode = (int)row["SortCode"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<DicItem> GetAllDicItems()
        {
            var list = new List<DicItem>();
            foreach (DataRow row in this["DicItem"].Rows)
            {
                var item = new DicItem
                {
                    Id = (Guid)row["Id"],
                    Code = (string)row["Code"],
                    DicId = (Guid)row["DicId"],
                    Name = (string)row["Name"],
                    IsEnabled = (int)row["IsEnabled"],
                    SortCode = (int)row["SortCode"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<EntityType> GetAllEntityTypes()
        {
            var list = new List<EntityType>();
            foreach (DataRow row in this["EntityType"].Rows)
            {
                var item = new EntityType
                {
                    Id = (Guid)row["Id"],
                    Code = (string)row["Code"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    DeveloperId = (Guid)row["DeveloperId"],
                    Codespace = (string)row["Codespace"],
                    DatabaseId = (Guid)row["DatabaseId"],
                    IsCatalogued = (bool)row["IsCatalogued"],
                    Name = (string)row["Name"],
                    SchemaName = row["SchemaName"] == DBNull.Value ? null : row["SchemaName"].ToString(),
                    TableName = row["TableName"] == DBNull.Value ? null : row["TableName"].ToString(),
                    EditHeight = (int)row["EditHeight"],
                    EditWidth = (int)row["EditWidth"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Property> GetAllProperties()
        {
            var list = new List<Property>();
            foreach (DataRow row in this["Property"].Rows)
            {
                var item = new Property
                {
                    Id = (Guid)row["Id"],
                    Code = (string)row["Code"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    Name = (string)row["Name"],
                    DicId = row["DicId"] == DBNull.Value ? null : (Guid?)row["DicId"],
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    EntityTypeId = (Guid)row["EntityTypeId"],
                    ForeignPropertyId = row["ForeignPropertyId"] == DBNull.Value ? null : (Guid?)row["ForeignPropertyId"],
                    GroupCode = row["GroupCode"] == DBNull.Value ? null : row["GroupCode"].ToString(),
                    GuideWords = row["GuideWords"] == DBNull.Value ? null : row["GuideWords"].ToString(),
                    InputType = row["InputType"] == DBNull.Value ? null : row["InputType"].ToString(),
                    IsDetailsShow = (bool)row["IsDetailsShow"],
                    IsDeveloperOnly = (bool)row["IsDeveloperOnly"],
                    IsInput = (bool)row["IsInput"],
                    IsTotalLine = (bool)row["IsTotalLine"],
                    MaxLength = row["MaxLength"] == DBNull.Value ? null : (int?)row["MaxLength"],
                    Tooltip = row["Tooltip"] == DBNull.Value ? null : row["Tooltip"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Function> GetAllFunctions()
        {
            var list = new List<Function>();
            foreach (DataRow row in this["Function"].Rows)
            {
                var item = new Function
                {
                    Id = (Guid)row["Id"],
                    Code = (string)row["Code"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    IsEnabled = (int)row["IsEnabled"],
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    IsManaged = (bool)row["IsManaged"],
                    DeveloperId = (Guid)row["DeveloperId"],
                    ResourceTypeId = (Guid)row["ResourceTypeId"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Group> GetAllGroups()
        {
            var list = new List<Group>();
            foreach (DataRow row in this["Group"].Rows)
            {
                var item = new Group
                {
                    Id = (Guid)row["Id"],
                    Name = (string)row["Name"],
                    ShortName = row["ShortName"] == DBNull.Value ? null : row["ShortName"].ToString(),
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    IsEnabled = (int)row["IsEnabled"],
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    CategoryCode = row["CategoryCode"] == DBNull.Value ? null : row["CategoryCode"].ToString(),
                    TypeCode = (string)row["TypeCode"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Menu> GetAllMenus()
        {
            var list = new List<Menu>();
            foreach (DataRow row in this["Menu"].Rows)
            {
                var item = new Menu
                {
                    Id = (Guid)row["Id"],
                    Name = (string)row["Name"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    AppSystemId = (Guid)row["AppSystemId"],
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    Url = row["Url"] == DBNull.Value ? null : row["Url"].ToString(),
                    ParentId = row["ParentId"] == DBNull.Value ? null : (Guid?)row["ParentId"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<UiView> GetAllUiViews()
        {
            var list = new List<UiView>();
            foreach (DataRow row in this["UiView"].Rows)
            {
                var item = new UiView
                {
                    Id = (Guid)row["Id"],
                    Etag = (byte[])row["Etag"],
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    Tooltip = row["Tooltip"] == DBNull.Value ? null : row["Tooltip"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<UiViewButton> GetAllUiViewButtons()
        {
            var list = new List<UiViewButton>();
            foreach (DataRow row in this["UiViewButton"].Rows)
            {
                var item = new UiViewButton
                {
                    Id = (Guid)row["Id"],
                    ButtonId = (Guid)row["ButtonId"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    FunctionId = row["FunctionId"] == DBNull.Value ? null : (Guid?)row["FunctionId"],
                    IsEnabled = (int)row["IsEnabled"],
                    UiViewId = (Guid)row["UiViewId"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Privilege> GetPrivileges()
        {
            var list = new List<Privilege>();
            foreach (DataRow row in this["Privilege"].Rows)
            {
                var item = new Privilege
                {
                    Id = (Guid)row["Id"],
                    SubjectType = (string)row["SubjectType"],
                    SubjectInstanceId = (Guid)row["SubjectInstanceId"],
                    ObjectType = (string)row["ObjectType"],
                    ObjectInstanceId = (Guid)row["ObjectInstanceId"],
                    AcContentType = row["AcContentType"] == DBNull.Value ? null : row["AcContentType"].ToString(),
                    AcContent = row["AcContent"] == DBNull.Value ? null : row["AcContent"].ToString(),
                    Etag = (byte[])row["Etag"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Role> GetAllRoles()
        {
            var list = new List<Role>();
            foreach (DataRow row in this["Role"].Rows)
            {
                var item = new Role
                {
                    Id = (Guid)row["Id"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    Name = row["Name"] == DBNull.Value ? null : row["Name"].ToString(),
                    Etag = (byte[])row["Etag"],
                    SortCode = (int)row["SortCode"],
                    CategoryCode = row["CategoryCode"] == DBNull.Value ? null : row["CategoryCode"].ToString(),
                    IsEnabled = (int)row["IsEnabled"],
                    NumberId = (int)row["NumberId"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<SsdSet> GetAllSsdSets()
        {
            var list = new List<SsdSet>();
            foreach (DataRow row in this["SsdSet"].Rows)
            {
                var item = new SsdSet
                {
                    Id = (Guid)row["Id"],
                    Name = row["Name"] == DBNull.Value ? null : row["Name"].ToString(),
                    Etag = (byte[])row["Etag"],
                    IsEnabled = (int)row["IsEnabled"],
                    SsdCard = (int)row["SsdCard"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<DsdSet> GetAllDsdSets()
        {
            var list = new List<DsdSet>();
            foreach (DataRow row in this["DsdSet"].Rows)
            {
                var item = new DsdSet
                {
                    Id = (Guid)row["Id"],
                    Name = row["Name"] == DBNull.Value ? null : row["Name"].ToString(),
                    Etag = (byte[])row["Etag"],
                    IsEnabled = (int)row["IsEnabled"],
                    DsdCard = (int)row["DsdCard"],
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<SsdRole> GetAllSsdRoles()
        {
            var list = new List<SsdRole>();
            foreach (DataRow row in this["SsdRole"].Rows)
            {
                var item = new SsdRole
                {
                    Id = (Guid)row["Id"],
                    RoleId = (Guid)row["RoleId"],
                    SsdSetId = (Guid)row["SsdSetId"],
                    Etag = (byte[])row["Etag"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<DsdRole> GetAllDsdRoles()
        {
            var list = new List<DsdRole>();
            foreach (DataRow row in this["DsdRole"].Rows)
            {
                var item = new DsdRole
                {
                    Id = (Guid)row["Id"],
                    RoleId = (Guid)row["RoleId"],
                    DsdSetId = (Guid)row["DsdSetId"],
                    Etag = (byte[])row["Etag"]
                };
                var entity = item as IEntityBase;
                entity.CreateBy = row["CreateBy"] == DBNull.Value ? null : row["CreateBy"].ToString();
                entity.CreateOn = row["CreateOn"] == DBNull.Value ? null : (DateTime?)row["CreateOn"];
                entity.CreateUserId = row["CreateUserId"] == DBNull.Value ? null : (Guid?)row["CreateUserId"];
                entity.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString();
                entity.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? null : (DateTime?)row["ModifiedOn"];
                entity.ModifiedUserId = row["ModifiedUserId"] == DBNull.Value ? null : (Guid?)row["ModifiedUserId"];

                list.Add(item);
            }
            return list;
        }

        public IList<Account> GetAllDevAccounts()
        {
            var list = new List<Account>();
            var properties = typeof(Account).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var columns = new DataColumn[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                DataColumn column = null;
                foreach (DataColumn item in this["Account"].Columns)
                {
                    if (item.ColumnName.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        column = item;
                        break;
                    }
                }
                if (column != null)
                {
                    columns[i] = column;
                }
            }
            foreach (DataRow row in this["Account"].Rows)
            {
                var obj = new Account();
                for (var i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (column == null)
                    {
                        continue;
                    }
                    var property = properties[i];
                    if (!property.CanWrite)
                    {
                        continue;
                    }
                    var value = row[column];
                    if (value == DBNull.Value) value = null;

                    property.SetValue(obj, value, null);
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
