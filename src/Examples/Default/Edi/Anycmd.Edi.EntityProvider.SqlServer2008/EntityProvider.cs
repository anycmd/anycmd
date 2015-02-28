
namespace Anycmd.Edi.EntityProvider.SqlServer2008
{
	using Engine.Edi;
	using Engine.Edi.Abstractions;
	using Engine.Host;
	using Engine.Host.Ac.Infra;
	using Engine.Host.Edi;
	using Engine.Host.Edi.Handlers;
	using Engine.Info;
	using Engine.Rdb;
	using Exceptions;
	using Query;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Data;
	using System.Data.Common;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Util;

	/// <summary>
	/// 基于SqlServer2008的数据提供程序
	/// </summary>
	[Export(typeof(IEntityProvider))]
	public sealed class EntityProvider : IEntityProvider
	{
		private static readonly Guid id = new Guid("A108FCAD-17A1-4F3C-A3F2-804131A0855A");
		private const string title = "数据提供程序SqlServer2008";
		private const string description = "使用SqlServer2008R2数据库";
		private const string author = "xuexs";

		private static readonly Dictionary<ElementDescriptor, ElementDataSchema>
			ElementDataSchemaDic = new Dictionary<ElementDescriptor, ElementDataSchema>();
		private static readonly Dictionary<OntologyDescriptor, RdbDescriptor> DbDic = new Dictionary<OntologyDescriptor, RdbDescriptor>();
		private static readonly object Locker = new object();

		private readonly ISqlFilterStringBuilder _filterStringBuilder = new SqlFilterStringBuilder();

		#region 属性
		/// <summary>
		/// 插件标识
		/// </summary>
		public Guid Id
		{
			get { return id; }
		}

		/// <summary>
		/// 标题
		/// </summary>
		public string Title
		{
			get { return title; }
		}

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get { return description; }
		}

		/// <summary>
		/// 作者。如xuexs
		/// </summary>
		public string Author
		{
			get { return author; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get { return this.Title; }
		}

		/// <summary>
		/// 
		/// </summary>
		public BuiltInResourceKind BuiltInResourceKind
		{
			get { return BuiltInResourceKind.EntityDbProvider; }
		}
		#endregion

		#region 本体元素数据模式
		/// <summary>
		/// 查看给定元素的本体元素数据模式
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public ElementDataSchema GetElementDataSchema(ElementDescriptor element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ElementDataSchema dataSchema;
			if (ElementDataSchemaDic.TryGetValue(element, out dataSchema))
			{
				return dataSchema;
			}
			else
			{
				var db = this.GetEntityDb(element.Ontology);
				DbTableColumn column;
				string columnId = string.Format("[{0}][{1}][{2}]", element.Ontology.Ontology.EntitySchemaName, element.Ontology.Ontology.EntityTableName, element.Element.FieldCode);
				if (!element.Host.Rdbs.DbTableColumns.TryGetDbTableColumn(db, columnId, out column))
				{
					var msg = "实体库中不存在" + columnId + "列";
					element.Host.LoggingService.Error(msg);
					return null;
				}
				dataSchema = new ElementDataSchema(column);
				ElementDataSchemaDic.Add(element, dataSchema);
				return dataSchema;
			}
		}
		#endregion

		#region 进程地址
		public string GetEntityDataSource(OntologyDescriptor ontology)
		{
			return this.GetEntityDb(ontology).IsLocalhost ? "localhost"
					: this.GetEntityDb(ontology).Database.DataSource;
		}
		#endregion

		#region 执行命令
		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public ProcessResult ExecuteCommand(DbCmd command)
		{
			try
			{
				if (command == null)
				{
					throw new ArgumentNullException("command");
				}
				if (command.Ontology == null)
				{
					throw new ArgumentNullException("ontology");
				}
				if (command.Client == null)
				{
					throw new ArgumentNullException("client");
				}
				var db = this.GetEntityDb(command.Ontology);
				Sql sqlObj = new Sql(command.Ontology, command.Client.Id.ToString(), command.CommandId, command.ActionType, command.LocalEntityId, command.InfoValue, db.CreateParameter);
				if (!sqlObj.IsValid)
				{
					return new ProcessResult(false, Status.ExecuteFail, sqlObj.Description);
				}
				else
				{
					if (!command.IsDumb)
					{
						int n = db.ExecuteNonQuery(sqlObj.Text, sqlObj.Parameters);
						if (n == 0)
						{
							if (command.ActionType == DbActionType.Insert)
							{
								return new ProcessResult(false, Status.AlreadyExist, "已经创建");
							}
							else
							{
								return new ProcessResult(false, Status.ExecuteFail, "目标记录不存在");
							}
						}
						else if (n > 1)
						{
							return new ProcessResult(new AnycmdException("Id:" + command.CommandId + ",意外的影响行数" + n.ToString()));
						}
					}
					return new ProcessResult(true, Status.ExecuteOk, "执行成功");
				}
			}
			catch (Exception ex)
			{
				command.Ontology.Host.LoggingService.Error(ex);
				return new ProcessResult(ex);
			}
		}
		#endregion

		#region ArchiveEntityDb

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="archive"></param>
		public void Archive(OntologyDescriptor ontology, IArchive archive)
		{
			var entityDb = this.GetEntityDb(ontology).Database;
			var archiveDb = this.GetArchiveDb(ontology, archive);
			if (archiveDb.Database.CatalogName.Equals(entityDb.CatalogName, StringComparison.OrdinalIgnoreCase))
			{
				throw new AnycmdException("归档库的数据库名不能与本体库相同");
			}
			// 创建归档库
			archiveDb.Create(this.GetEntityDb(ontology), ontology.Host.Config.EntityArchivePath);
			string archiveTableName = string.Format(
				"{0}.{1}.{2}", archiveDb.Database.CatalogName, ontology.Ontology.EntitySchemaName, ontology.Ontology.EntityTableName);
			string sql =
@"if OBJECT_ID('" + archiveTableName + "') IS NULL select * into " + archiveTableName
+ " from " + ontology.Ontology.EntityTableName;
			// 执行select into归档数据
			this.GetEntityDb(ontology).ExecuteNonQuery(sql, null);
		}
		#endregion

		#region DropArchivedEntityDb
		/// <summary>
		/// 删除归档数据
		/// </summary>
		/// <param name="archive"></param>
		public void DropArchive(ArchiveState archive)
		{
			var catalogName = string.Format(
							"Archive{0}{1}_{2}",
							archive.Ontology.Ontology.Code,
							archive.ArchiveOn.ToString("yyyyMMdd"),
							archive.NumberId.ToString());
			if (catalogName.Equals(this.GetEntityDb(archive.Ontology).Database.CatalogName, StringComparison.OrdinalIgnoreCase))
			{
				throw new AnycmdException("归档库的数据库名不能与本体库相同");
			}
			string sql =
@"IF EXISTS ( SELECT  1
			FROM    master..sysdatabases
			WHERE   name = '" + catalogName + @"' ) 
	ALTER DATABASE " + catalogName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE
IF EXISTS ( SELECT  1
			FROM    master..sysdatabases
			WHERE   name = '" + catalogName + @"' ) 
	ALTER DATABASE " + catalogName + @" SET MULTI_USER
IF EXISTS ( SELECT  1
			FROM    master..sysdatabases
			WHERE   name = '" + catalogName + @"' ) 
	DROP DATABASE " + catalogName + "";
			this.GetEntityDb(archive.Ontology).ExecuteNonQuery(sql, null);
		}
		#endregion

		#region GetTopTwo
		public TowInfoTuple GetTopTwo(
			OntologyDescriptor ontology, IEnumerable<InfoItem> infoIDs, OrderedElementSet selectElements)
		{
			return GetTop2InfoItemSet(ontology, this.GetEntityDb(ontology), infoIDs, selectElements);
		}

		private TowInfoTuple GetTop2InfoItemSet(
			ArchiveState archive, IEnumerable<InfoItem> infoIDs, OrderedElementSet selectElements)
		{
			return GetTop2InfoItemSet(archive.Ontology, this.GetArchiveDb(archive.Ontology, archive), infoIDs, selectElements);
		}
		#endregion

		#region Get

		/// <summary>
		/// 获取给定本体码和存储标识的本节点的数据
		/// <remarks>本节点通常是中心节点</remarks>
		/// </summary>
		/// <param name="ontology">本体</param>
		/// <param name="localEntityId"></param>
		/// <param name="selectElements"></param>
		/// <returns>数据记录，表现为字典形式，键是数据元素编码值是相应数据元素对应的数据项值</returns>
		public InfoItem[] Get(
			OntologyDescriptor ontology, string localEntityId, OrderedElementSet selectElements)
		{
			var topTwo = GetTopTwo(ontology, new InfoItem[] { InfoItem.Create(ontology.IdElement, localEntityId) }, selectElements);
			if (topTwo.BothHasValue || topTwo.BothNoValue)
			{
				return new InfoItem[0];
			}
			else
			{
				return topTwo.SingleInfoTuple;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="localEntityId"></param>
		/// <param name="selectElements"></param>
		/// <returns></returns>
		public InfoItem[] Get(
			ArchiveState archive, string localEntityId, OrderedElementSet selectElements)
		{
			var topTwo = GetTop2InfoItemSet(archive, new InfoItem[] { InfoItem.Create(archive.Ontology.IdElement, localEntityId) }, selectElements);
			if (topTwo.BothHasValue || topTwo.BothNoValue)
			{
				return new InfoItem[0];
			}
			else
			{
				return topTwo.SingleInfoTuple;
			}
		}
		#endregion

		#region GetList
		/// <summary>
		/// 获取给定实体标识列表中的标识标识的每一个实体元组
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="selectElements"></param>
		/// <param name="entityIDs"></param>
		/// <returns></returns>
		public DataTuple GetList(OntologyDescriptor ontology, OrderedElementSet selectElements, List<string> entityIDs)
		{
			if (ontology == null)
			{
				throw new ArgumentNullException("ontology");
			}
			if (selectElements == null)
			{
				throw new ArgumentNullException("selectElements");
			}
			if (entityIDs == null || entityIDs.Count == 0)
			{
				throw new ArgumentException("entityId");
			}
			var ids = new StringBuilder("(");
			int len = ids.Length;
			foreach (var id in entityIDs)
			{
				if (ids.Length != len)
				{
					ids.Append(",");
				}
				ids.Append("'").Append(id).Append("'");
			}
			ids.Append(")");
			var tableName = ontology.Ontology.EntityTableName;
			if (selectElements.Count == 0)
			{
				selectElements.Add(ontology.Elements[ontology.IdElement.Element.Code]);
			}
			var sqlQuery = new StringBuilder("select ");
			int l = sqlQuery.Length;
			foreach (var item in selectElements)
			{
				if (sqlQuery.Length != l)
				{
					sqlQuery.Append(",");
				}
				sqlQuery.Append("t.[").Append(item.Element.Code).Append("]");
			}
			sqlQuery.Append(" from [").Append(tableName).Append("] as t where t.[")
				.Append(ontology.IdElement.Element.Code).Append("] in ").Append(ids.ToString());// TODO:参数化EntityIDs
			var list = new List<object[]>();
			var reader = this.GetEntityDb(ontology).ExecuteReader(sqlQuery.ToString());
			while (reader.Read())
			{
				var values = new object[selectElements.Count];
				for (int i = 0; i < selectElements.Count; i++)
				{
					values[i] = reader.GetValue(i);
				}
				list.Add(values);
			}
			reader.Close();

			return new DataTuple(selectElements, list.ToArray());
		}
		#endregion

		#region GetPlist

		/// <summary>
		/// 按照目录分页获取指定节点、本体的数据
		/// <remarks>
		/// 如果传入的目录为空则表示获取全部目录的数据
		/// </remarks>
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="selectElements"></param>
		/// <param name="filters">过滤器列表</param>
		/// <param name="pagingData"></param>
		/// <returns></returns>
		public DataTuple GetPlist(
			OntologyDescriptor ontology,
			OrderedElementSet selectElements,
			List<FilterData> filters,
			PagingInput pagingData)
		{
			return this.GetPlistInfoItems(
				ontology, this.GetEntityDb(ontology),
				selectElements,
				filters, pagingData);
		}

		public DataTuple GetPlist(
			ArchiveState archive,
			OrderedElementSet selectElements,
			List<FilterData> filters,
			PagingInput pagingData)
		{
			return this.GetPlistInfoItems(
				archive.Ontology, this.GetArchiveDb(archive.Ontology, archive),
				selectElements,
				filters, pagingData);
		}
		#endregion

		#region private method
		#region GetTop2InfoItemSet

		/// <summary>
		/// 根据给定的本体码和信息标识获取本节点两条数据
		/// </summary>
		/// <param name="ontology">本体</param>
		/// <param name="db"></param>
		/// <param name="infoIds">多列联合信息标识字典，键必须不区分大小写</param>
		/// <param name="selectElements">选择元素</param>
		/// <returns></returns>
		private TowInfoTuple GetTop2InfoItemSet(OntologyDescriptor ontology,
			RdbDescriptor db, IEnumerable<InfoItem> infoIds, OrderedElementSet selectElements)
		{
			if (infoIds == null || !infoIds.Any())
			{
				return new TowInfoTuple(null, null);
			}

			var sb = new StringBuilder();
			var sqlParameters = new List<DbParameter>();
			var elementList = selectElements.ToList();
			sb.Append("select top 2 ");
			int l = sb.Length;
			foreach (var element in elementList)
			{
				if (sb.Length != l)
				{
					sb.Append(",");
				}
				sb.Append("t.[").Append(element.Element.FieldCode).Append("]");
			}
			sb.Append(" from [").Append(ontology.Ontology.EntityTableName).Append("] as t where");
			if (ontology.Ontology.IsLogicalDeletionEntity)
			{
				sb.Append(" t.DeletionStateCode=0 ");
			}
			else
			{
				sb.Append(" 1=1 ");
			}
			foreach (var element in infoIds)
			{
				sb.Append(" and t.[").Append(element.Element.Element.FieldCode)
					.Append("]=").Append("@").Append(element.Element.Element.FieldCode);
				object obj = element.Value;
				if (obj == null)
				{
					obj = DBNull.Value;
				}
				sqlParameters.Add(CreateParameter(db, element.Element.Element.FieldCode, obj, DbType.String));
			}
			var infoValue1 = new List<InfoItem>();
			var infoValue2 = new List<InfoItem>();
			using (var reader = db.ExecuteReader(sb.ToString(), sqlParameters.ToArray()))
			{
				if (reader.Read())
				{
					for (int i = 0; i < elementList.Count; i++)
					{
						infoValue1.Add(InfoItem.Create(elementList[i], reader.GetValue(i).ToString()));
					}
				}
				if (reader.Read())
				{
					for (int i = 0; i < elementList.Count; i++)
					{
						infoValue2.Add(InfoItem.Create(elementList[i], reader.GetValue(i).ToString()));
					}
				}
				reader.Close();
			}

			return new TowInfoTuple(infoValue1.ToArray(), infoValue2.ToArray());
		}
		#endregion

		#region GetPlistInfoItems

		/// <summary>
		/// 根据目录获取给定节点和本体的数据，如果传入的目录为空表示获取本节点的数据
		/// <remarks>本节点通常是中心节点</remarks>
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="db">模型</param>
		/// <param name="filters"></param>
		/// <param name="selectElements">sql select语句的选择列集合</param>
		/// <param name="pagingData"></param>
		/// <returns>
		/// 数据记录列表，数据记录表现为字典形式，键是数据元素编码值是相应数据元素对应的数据项值
		/// </returns>
		private DataTuple GetPlistInfoItems(
			OntologyDescriptor ontology,
			RdbDescriptor db, OrderedElementSet selectElements, List<FilterData> filters,
			PagingInput pagingData)
		{
			if (string.IsNullOrEmpty(pagingData.SortField))
			{
				pagingData.SortField = "IncrementId";
			}
			if (string.IsNullOrEmpty(pagingData.SortOrder))
			{
				pagingData.SortOrder = "asc";
			}

			var elements = ontology.Elements;
			if (filters != null)
			{
				for (int i = 0; i < filters.Count; i++)
				{
					var filter = filters[i];
					if (elements.ContainsKey(filter.field))
					{
						// TODO:根据数据属性优化查询，比如对于身份证件号来说如果输入的值长度
						// 为20或18的话可以将like替换为等于
						filter.type = "string";
						var element = elements[filter.field];
						if (element.Element.IsEnabled != 1)
						{
							continue;
						}
						if (element.Element.InfoDicId.HasValue)
						{
							filter.comparison = "eq";
						}
						else
						{
							filter.comparison = "like";
						}
					}
					else
					{
						filters.RemoveAt(i);
					}
				}
			}

			var tableName = ontology.Ontology.EntityTableName;
			var sbSqlPredicate = new StringBuilder();
			var l = sbSqlPredicate.Length;

			var pQueryList = new List<DbParameter>();
			List<DbParameter> pFilters;
			var filterString = _filterStringBuilder.FilterString(db, filters, null, out pFilters);
			if (!string.IsNullOrEmpty(filterString))
			{
				foreach (var pFilter in pFilters)
				{
					object obj = pFilter.Value;
					if (obj == null)
					{
						obj = DBNull.Value;
					}
					var p = db.CreateParameter();
					p.ParameterName = pFilter.ParameterName;
					p.Value = obj;
					pQueryList.Add(p);
				}
				if (sbSqlPredicate.Length != l)
				{
					sbSqlPredicate.Append(" and ");
				}
				sbSqlPredicate.Append(filterString);
			}

			string sqlPredicateString = string.Empty;
			if (sbSqlPredicate.Length > 0)
			{
				sqlPredicateString = sbSqlPredicate.ToString();
			}
			var sqlText = new StringBuilder();
			OrderedElementSet elementList;
			if (selectElements == null || selectElements.Count == 0)
			{
				elementList = new OrderedElementSet { ontology.Elements["id"] };
			}
			else
			{
				elementList = selectElements;
			}
			sqlText.Append("SELECT TOP {0} ");
			int len = sqlText.Length;

			foreach (var element in elementList)
			{
				if (sqlText.Length != len)
				{
					sqlText.Append(",");
				}
				sqlText.Append("[").Append(element.Element.FieldCode).Append("]");
			}

			sqlText.Append(" FROM (SELECT ROW_NUMBER() OVER(ORDER BY {1} {2}) AS RowNumber,");
			len = sqlText.Length;

			foreach (var element in elementList)
			{
				if (sqlText.Length != len)
				{
					sqlText.Append(",");
				}
				sqlText.Append("[").Append(element.Element.FieldCode).Append("]");
			}

			sqlText.Append(" FROM {3} where ");
			if (ontology.Ontology.IsLogicalDeletionEntity)
			{
				sqlText.Append("DeletionStateCode = 0");
			}
			else
			{
				sqlText.Append("1 = 1");
			}
			if (!string.IsNullOrEmpty(sqlPredicateString))
			{
				sqlText.Append(" and ").Append(sqlPredicateString);
			}
			sqlText.Append(") a WHERE a.RowNumber > {4}");
			string sqlQuery = string.Format(
				sqlText.ToString(),
				pagingData.PageSize.ToString(CultureInfo.InvariantCulture),
				pagingData.SortField,
				pagingData.SortOrder,
				tableName,
				(pagingData.SkipCount).ToString(CultureInfo.InvariantCulture));

			pagingData.Count(() =>
			{
				string where = ontology.Ontology.IsLogicalDeletionEntity ? "where DeletionStateCode = 0" : "";
				string sqlCount = string.Format("select count(1) from {0} {1}", tableName, where);
				if (!string.IsNullOrEmpty(sqlPredicateString))
				{
					sqlCount = sqlCount + " and " + sqlPredicateString;
				}
				return (int)db.ExecuteScalar(
					sqlCount, pQueryList.Select(p => ((ICloneable)p).Clone()).Cast<DbParameter>().ToArray());
			});

			var list = new List<object[]>();
			var reader = db.ExecuteReader(sqlQuery, pQueryList.ToArray());
			while (reader.Read())
			{
				var values = new object[elementList.Count];
				for (int i = 0; i < elementList.Count; i++)
				{
					values[i] = reader.GetValue(i);
				}
				list.Add(values);
			}
			reader.Close();

			return new DataTuple(elementList, list.ToArray());
		}
		#endregion

		#region GetEntityDb
		/// <summary>
		/// 
		/// </summary>
		private RdbDescriptor GetEntityDb(OntologyDescriptor ontology)
		{
			if (!DbDic.ContainsKey(ontology))
			{
				lock (Locker)
				{
					if (!DbDic.ContainsKey(ontology))
					{
						RdbDescriptor db;
						if (!ontology.Host.Rdbs.TryDb(ontology.Ontology.EntityDatabaseId, out db))
						{
							throw new AnycmdException("意外的数据库Id" + ontology.Ontology.EntityDatabaseId.ToString());
						}
						DbDic.Add(ontology, db);
					}
				}
			}
			return DbDic[ontology];
		}
		#endregion

		#region GetArchiveDb
		private RdbDescriptor GetArchiveDb(OntologyDescriptor ontology, IArchive archive)
		{
			var entityDb = this.GetEntityDb(ontology).Database;
			var catalogName = string.Format(
							"Archive{0}{1}_{2}",
							ontology.Ontology.Code,
							archive.ArchiveOn.ToString("yyyyMMdd"),
							archive.NumberId.ToString());
			var datasource = archive.DataSource;
			var userId = archive.UserId;
			var password = archive.Password;
			if (string.IsNullOrEmpty(datasource))
			{
				datasource = entityDb.DataSource;
			}
			if (string.IsNullOrEmpty(userId))
			{
				userId = entityDb.UserId;
			}
			if (string.IsNullOrEmpty(password))
			{
				password = entityDb.Password;
			}

			return new RdbDescriptor(ontology.Host, new RDatabase
			{
				CatalogName = catalogName,
				CreateBy = archive.CreateBy,
				CreateOn = DateTime.Now,
				CreateUserId = archive.CreateUserId,
				DataSource = datasource,
				Description = ontology.Ontology.Name + "归档",
				Id = Guid.NewGuid(),
				IsTemplate = false,
				Password = password,
				Profile = entityDb.Profile,
				ProviderName = entityDb.ProviderName,
				RdbmsType = archive.RdbmsType,
				UserId = userId
			});
		}
		#endregion
		#endregion

		private DbParameter CreateParameter(RdbDescriptor rdb, string parameterName, object value, DbType dbType)
		{
			var p = rdb.CreateParameter();
			p.ParameterName = parameterName;
			p.Value = value;
			p.DbType = dbType;

			return p;
		}

		#region 内部类 Sql
		/// <summary>
		/// sql语句模型
		/// </summary>
		private sealed class Sql
		{
			readonly OntologyDescriptor _ontology;
			readonly string _localEntityId;
			readonly string _clientId;
			readonly string _commandId;
			readonly InfoItem[] _infoValue;
			private bool _isValid = true;
			private string _description = string.Empty;
			private readonly StringBuilder _text = new StringBuilder();
			private readonly Dictionary<string, DbParameter> _parameters = new Dictionary<string, DbParameter>(StringComparer.OrdinalIgnoreCase);
			private readonly Func<DbParameter> _createParameter;

			private Sql() { }

			/// <summary>
			/// 
			/// </summary>
			public Sql(OntologyDescriptor ontology, string clientId, string commandId, DbActionType actionType, string localEntityId, InfoItem[] infoValue, Func<DbParameter> createParameter)
			{
				this._ontology = ontology;
				this._localEntityId = localEntityId;
				this._infoValue = infoValue;
				this._clientId = clientId;
				this._commandId = commandId;
				this._createParameter = createParameter;
				if (ontology == null
					|| string.IsNullOrEmpty(localEntityId))
				{
					this.IsValid = false;
					this.Description = "命令信息标识或信息值为空或本体为空或本地标识为空";
					throw new ArgumentNullException("command");
				}
				// 无需把switch流程选择逻辑重构掉，因为actionType枚举不存在变化
				switch (actionType)
				{
					case DbActionType.Insert:
						if (infoValue == null || infoValue.Length == 0)
						{
							this.Description = "命令信息值为空";
							break;
						}
						BuildInsertSql();
						break;
					case DbActionType.Update:
						if (infoValue == null || infoValue.Length == 0)
						{
							this.Description = "命令信息值为空";
							break;
						}
						BuildUpdateSql();
						break;
					case DbActionType.Delete:
						BuildDeleteSql();
						break;
					default:
						this.IsValid = false;
						this.Description = "意外的不能执行的动作码" + actionType.ToString();
						break;
				}
			}

			#region 属性
			/// <summary>
			/// 
			/// </summary>
			public bool IsValid
			{
				get
				{
					return _isValid;
				}
				private set
				{
					_isValid = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public string Description
			{
				get { return _description; }
				private set
				{
					_description = value;
				}
			}

			/// <summary>
			/// sql语句
			/// </summary>
			public string Text
			{
				get
				{
					return _text.ToString();
				}
			}

			/// <summary>
			/// 参数数组
			/// </summary>
			public DbParameter[] Parameters
			{
				get
				{
					return _parameters.Values.ToArray();
				}
			}
			#endregion

			#region private methods
			#region BuildInsertSql
			/// <summary>
			/// 构建create语句
			/// </summary>
			/// <returns></returns>
			private void BuildInsertSql()
			{
				IList<InfoItem> valueItems = new List<InfoItem>();
				valueItems.Add(InfoItem.Create(_ontology.IdElement, _localEntityId));
				valueItems.Add(InfoItem.Create(_ontology.CreateNodeIdElement, _clientId));
				valueItems.Add(InfoItem.Create(_ontology.ModifiedNodeIdElement, _clientId));
				valueItems.Add(InfoItem.Create(_ontology.CreateCommandIdElement, _commandId));
				valueItems.Add(InfoItem.Create(_ontology.ModifiedCommandIdElement, _commandId));
				foreach (var item in _infoValue)
				{
					valueItems.Add(item);
				}

				_text.Append("insert into ").Append(_ontology.Ontology.EntityTableName).Append("(");
				int l1 = _text.Length;
				foreach (var valueItem in valueItems)
				{
					if (_text.Length > l1)
					{
						_text.Append(",");
					}
					_text.Append(valueItem.Element.Element.FieldCode);
				}
				_text.Append(") SELECT ");
				l1 = _text.Length;
				foreach (var valueItem in valueItems)
				{
					if (_text.Length > l1)
					{
						_text.Append(",");
					}
					_text.Append("@").Append(valueItem.Element.Element.FieldCode);
				}
				_text.Append(" WHERE   NOT EXISTS ( SELECT ");
				l1 = _text.Length;
				foreach (var valueItem in valueItems)
				{
					if (_text.Length > l1)
					{
						_text.Append(",");
					}
					_text.Append(valueItem.Element.Element.FieldCode);
				}
				_text.Append(" FROM ").Append(_ontology.Ontology.EntityTableName);
				_text.Append(@" WHERE  Id = @Id);");
				foreach (var value in valueItems)
				{
					object obj = value.Value;
					if (value.Element.DataSchema.TypeName.StartsWith("date"))
					{
						DateTime t;
						if (!DateTime.TryParse(value.Value, out t))
						{
							this.IsValid = false;
							this.Description += value.Key + "非法的日期格式" + value.Value + ";";
						}
						if (t < SystemTime.MinDate)
						{
							t = SystemTime.MinDate;
						}
						else if (t > SystemTime.MaxDate)
						{
							t = SystemTime.MaxDate;
						}
						obj = t;
					}
					if (obj == null)
					{
						obj = DBNull.Value;
					}
					var p = _createParameter();
					p.ParameterName = value.Key;
					p.Value = obj;
					_parameters.Add(value.Key, p);
				}
			}
			#endregion

			#region BuildUpdateSql
			/// <summary>
			/// 构建update语句
			/// </summary>
			/// <returns></returns>
			private void BuildUpdateSql()
			{
				IList<InfoItem> valueItems = new List<InfoItem>();
				// 添加触发器关注字段
				valueItems.Add(InfoItem.Create(_ontology.ModifiedOnElement, DateTime.Now.ToString()));
				valueItems.Add(InfoItem.Create(_ontology.ModifiedNodeIdElement, _clientId));
				valueItems.Add(InfoItem.Create(_ontology.ModifiedCommandIdElement, _commandId));
				foreach (var item in _infoValue)
				{
					valueItems.Add(item);
				}
				_text.Append("update [").Append(_ontology.Ontology.EntityTableName).Append("] set ");
				int l2 = _text.Length;
				foreach (var infoItem in valueItems)
				{
					if (infoItem.Element != _ontology.IdElement)
					{
						if (_text.Length > l2)
						{
							_text.Append(",");
						}
						_text.Append(infoItem.Element.Element.FieldCode).Append("=");
						this.Append(infoItem);
					}
				}

				_text.Append(" where Id=");
				this.Append(InfoItem.Create(_ontology.IdElement, _localEntityId));
			}
			#endregion

			#region BuildDeleteSql
			/// <summary>
			/// 构建delete语句
			/// </summary>
			/// <returns></returns>
			private void BuildDeleteSql()
			{
				if (_ontology.Ontology.IsLogicalDeletionEntity)
				{
					_text.Append("update [").Append(
						_ontology.Ontology.EntityTableName).Append("] set DeletionStateCode=1, ")
						.Append(_ontology.ModifiedNodeIdElement.Element.FieldCode)
						.Append("='" + _clientId + "',")
						.Append(_ontology.ModifiedCommandIdElement.Element.FieldCode)
						.Append("='" + _commandId + "' where Id=");
				}
				else
				{
					_text.Append("delete [").Append(_ontology.Ontology.EntityTableName).Append("] where Id=");
				}
				this.Append(InfoItem.Create(_ontology.IdElement, _localEntityId));
			}
			#endregion

			#region PrepareSql

			/// <summary>
			/// 
			/// </summary>
			/// <param name="value"></param>
			private void Append(InfoItem value)
			{
				if (!_parameters.ContainsKey(value.Key))
				{
					object obj = value.Value;
					if (value.Element.DataSchema.TypeName.StartsWith("date"))
					{
						DateTime t;
						if (!DateTime.TryParse(value.Value, out t))
						{
							this.IsValid = false;
							this.Description += value.Key + "非法的日期格式" + value.Value + ";";
						}
						if (t < SystemTime.MinDate)
						{
							t = SystemTime.MinDate;
						}
						else if (t > SystemTime.MaxDate)
						{
							t = SystemTime.MaxDate;
						}
						obj = t;
					}
					if (obj == null)
					{
						obj = DBNull.Value;
					}
					var p = _createParameter();
					p.ParameterName = value.Key;
					p.Value = obj;
					_parameters.Add(value.Key, p);
					_text.Append("@").Append(value.Key);
				}
			}
			#endregion

			#endregion
		}
		#endregion
	}
}
