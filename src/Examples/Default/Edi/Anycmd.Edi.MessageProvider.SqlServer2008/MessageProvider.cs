
namespace Anycmd.Edi.MessageProvider.SqlServer2008
{
    using Engine.Edi;
    using Engine.Hecp;
    using Engine.Host;
    using Engine.Host.Edi;
    using Engine.Host.Edi.Handlers;
    using Engine.Info;
    using Engine.Rdb;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using Util;

    /// <summary>
    /// 基于SqlServer2008r2实现的命令提供程序。2005、2008、2012版本的SqlServer都兼容。
    /// </summary>
    [Export(typeof(IMessageProvider))]
    public sealed class MessageProvider : IMessageProvider
    {
        #region Private Fields
        private static readonly Guid id = new Guid("0B7AE563-CC76-4B2D-955F-65F498AF9FA3");
        private static readonly string title = "命令提供程序SqlServer2008";
        private static readonly string description = "使用SqlServer208R2数据库";
        private static readonly string author = "xuexs";
        private static readonly Dictionary<OntologyDescriptor, RdbDescriptor> _dbDic = new Dictionary<OntologyDescriptor, RdbDescriptor>();
        private static object _locker = new object();
        #endregion

        #region 插件信息
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
        #endregion

        #region 资源
        public string Name
        {
            get { return this.Title; }
        }

        public BuiltInResourceKind BuiltInResourceKind
        {
            get { return BuiltInResourceKind.CommandDbProvider; }
        }
        #endregion

        #region SaveCommand
        /// <summary>
        /// 保存命令事件
        /// </summary>
        /// <param name="ontology"></param>
        /// <param name="commandEvent"></param>
        public ProcessResult SaveCommand(OntologyDescriptor ontology, MessageEntity command)
        {
            if (ontology == null)
            {
                return new ProcessResult(new ArgumentNullException("ontology", "ontology参数为null"));
            }
            if (command == null)
            {
                return new ProcessResult(new ArgumentNullException("command"));
            }

            return this.SaveCommands(ontology, new MessageEntity[] { command });
        }
        #endregion

        #region SaveCommands

        /// <summary>
        /// 批量保存本地事件
        /// </summary>
        /// <param name="ontology">本体</param>
        /// <param name="commands"></param>
        public ProcessResult SaveCommands(OntologyDescriptor ontology, MessageEntity[] commands)
        {
            if (ontology == null)
            {
                return new ProcessResult(new ArgumentNullException("ontology", @"ontology参数为null"));
            }
            if (commands == null)
            {
                return new ProcessResult(new ArgumentNullException("commands"));
            }
            foreach (var command in commands)
            {
                if (command == null)
                {
                    return new ProcessResult(new ArgumentNullException("command", "命令数组中有null"));
                }
                else if (!ontology.Ontology.Code.Equals(command.Ontology, StringComparison.OrdinalIgnoreCase))
                {
                    return new ProcessResult(new ArgumentNullException("command.Ontology", "同一命令数组中的命令必须在相同本体下"));
                }
            }
            ProcessResult r = ProcessResult.Ok;
            // 存放异常信息
            StringBuilder sb = new StringBuilder();
            int l = sb.Length;
            // 按照命令类型分组，不同类型的命令可能存储在不同的数据库表
            var gs = commands.GroupBy(c => c.CommandType);
            foreach (var g in gs)
            {
                // 本组命令类型所对应的数据库表
                string tableId = string.Format("[{0}][{1}]", ontology.Ontology.MessageSchemaName, GetTableName(g.Key));
                DbTable dbTable;
                if (!this.GetCommandDb(ontology).TryGetDbTable(tableId, out dbTable))
                {
                    r = new ProcessResult(new AnycmdException("意外的数据库表标识" + tableId));
                }
                // 当前命令表模式克隆得到的新表
                var dt = this.GetCommandDb(ontology).NewTable(dbTable);
                foreach (var command in g)
                {
                    if (command.CommandType == MessageTypeKind.Invalid || command.CommandType == MessageTypeKind.AnyCommand)
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "Invalid和AnyCommand类型命令不能持久化");
                    }
                    else if (string.IsNullOrEmpty(command.Verb.Code))
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "Verb为空或null不能持久化");
                    }
                    else if (string.IsNullOrEmpty(command.MessageId)
                        || command.MessageId.Length > 50)
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "MessageID为空或null或者长度大于50字符不能持久化");
                    }
                    else if (string.IsNullOrEmpty(command.LocalEntityId))
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "LocalEntityID为空或null不能持久化");
                    }
                    else if (command.DataTuple.IdItems.IsEmpty)
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "当前命令的信息标识为空则不能持久化");
                    }
                    else if (!command.TimeStamp.IsValid())
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "TimeStamp非法则不能持久化");
                    }
                    else if (string.IsNullOrEmpty(command.Ontology))
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "Ontology为空或null不能持久化");
                    }
                    else if (string.IsNullOrEmpty(command.ClientId))
                    {
                        r = new ProcessResult(false, Status.InvalidArgument, "ClientID为空或null不能持久化");
                    }
                    if (r.IsSuccess)
                    {
                        // 将当前命令转化DataRow，一个命令对应一行
                        var row = command.ToDataRow(dt);
                        int ll = sb.Length;
                        // 检测每一列对应的值是否超出了数据库定义的长度
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            var dbCol = dt.Columns[i];
                            if (dbCol.MaxLength != -1)
                            {
                                if (row[i].ToString().Length > dbCol.MaxLength)
                                {
                                    if (sb.Length != ll)
                                    {
                                        sb.Append(";");
                                    }
                                    sb.Append(tableId).Append("表:");
                                    sb.Append(dbCol.ColumnName + "超过最大长度，最长" + dbCol.MaxLength.ToString());
                                }
                            }
                        }
                        if (sb.Length == ll)
                        {
                            // 如果不是哑命令。哑命令是不能爆炸的。
                            if (!command.IsDumb)
                            {
                                dt.Rows.Add(row);
                            }
                        }
                    }
                    else
                    {
                        sb.Append(r.Description);
                    }
                }

                this.GetCommandDb(ontology).WriteToServer(dt);
            }

            if (sb.Length != l)
            {
                r = new ProcessResult(new AnycmdException(sb.ToString()));
            }
            else
            {
                r = ProcessResult.Ok;
            }

            if (r.Exception != null)
            {
                ontology.Host.LoggingService.Error(r.Exception);
            }

            return r;
        }
        #endregion

        #region DeleteCommand
        public ProcessResult DeleteCommand(MessageTypeKind commandType, OntologyDescriptor ontology, Guid id, bool isDumb)
        {
            ProcessResult r;
            if (isDumb)
            {
                r = ProcessResult.Ok;
            }
            else
            {
                var queryString =
    @"delete " + GetTableName(commandType) + " where Id=@Id";
                int n = this.GetCommandDb(ontology).ExecuteNonQuery(queryString, new SqlParameter("Id", id));
                if (n == 1)
                {
                    r = ProcessResult.Ok;
                }
                else
                {
                    r = new ProcessResult(new AnycmdException("意外的影响行数" + n.ToString()));
                }
            }

            if (r.Exception != null)
            {
                ontology.Host.LoggingService.Error(r.Exception);
            }

            return r;
        }
        #endregion

        #region GetTopNCommandEntities
        /// <summary>
        /// 获取给定条目的给定类型的命令
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="ontology"></param>
        /// <param name="n">条数</param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns>命令消息集合</returns>
        public IList<MessageEntity> GetTopNCommands(MessageTypeKind commandType, OntologyDescriptor ontology, int n, string sortField, string sortOrder)
        {
            if (ontology == null)
            {
                return new List<MessageEntity>();
            }
            if (commandType == MessageTypeKind.Invalid || commandType == MessageTypeKind.AnyCommand)
            {
                return new List<MessageEntity>();
            }
            var sql =
@"select top " + n.ToString() +
" * from [" + GetTableName(commandType) + "] as c where lower(c.Ontology)=@Ontology order by c." + sortField + " " + sortOrder;
            var pOntology = new SqlParameter("Ontology", ontology.Ontology.Code.ToLower());
            IList<MessageEntity> list = new List<MessageEntity>();
            using (var reader = this.GetCommandDb(ontology).ExecuteReader(sql, pOntology))
            {
                while (reader.Read())
                {
                    list.Add(CommandRecord.Create(ontology.Host, commandType, reader));
                }
                reader.Close();

                return list;
            }
        }
        #endregion

        #region GetPlistCommands
        /// <summary>
        /// 根据节点分页获取命令
        /// </summary>
        /// <typeparam name="T">命令类型参数</typeparam>
        /// <param name="ontology">本体</param>
        /// <param name="catalogCode">目录码</param>
        /// <param name="actionCode">动作码，空值表示忽略本查询条件</param>
        /// <param name="nodeId">节点标识，空值表示忽略本查询条件</param>
        /// <param name="infoId">信息标识，空值表示忽略本查询条件</param>
        /// <param name="infoValue">信息值，空值表示忽略本查询条件</param>
        /// <param name="localEntityId">本地实体标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        public IList<MessageEntity> GetPlistCommands(MessageTypeKind commandType,
            OntologyDescriptor ontology, string catalogCode, string actionCode, Guid? nodeId, string localEntityId,
            int pageIndex, int pageSize, string sortField, string sortOrder, out Int64 total)
        {
            var tableName = GetTableName(commandType);
            var queryString =
@"select top " + pageSize.ToString() + " * from (SELECT ROW_NUMBER() OVER(ORDER BY " + sortField + " " + sortOrder + ") AS RowNumber,* from " + tableName +
@" as a where a.Ontology=@Ontology {0}) b 
 where b.RowNumber>" + (pageSize * pageIndex).ToString();
            var countQS =
@"select count(Id) from " + tableName + @" as a 
where a.Ontology=@Ontology {0}";
            if (!string.IsNullOrEmpty(actionCode))
            {
                queryString = string.Format(queryString, " and a.Verb=@Verb {0}");
                countQS = string.Format(countQS, " and a.Verb=@Verb {0}");
            }
            if (!string.IsNullOrEmpty(localEntityId))
            {
                queryString = string.Format(queryString, " and a.LocalEntityID=@LocalEntityId {0}");
                countQS = string.Format(countQS, " and a.LocalEntityID=@LocalEntityId {0}");
            }
            if (!string.IsNullOrEmpty(catalogCode))
            {
                queryString = string.Format(queryString, " and a.CatalogCode like @CatalogCode {0}");
                countQS = string.Format(countQS, " and a.CatalogCode like @CatalogCode {0}");
            }
            if (nodeId.HasValue)
            {
                queryString = string.Format(queryString, " and a.ClientID=@ClientId");
                countQS = string.Format(countQS, " and a.ClientID=@ClientId");
            }
            else
            {
                queryString = string.Format(queryString, "");
                countQS = string.Format(countQS, "");
            }

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("Ontology", ontology.Ontology.Code));
            if (!string.IsNullOrEmpty(actionCode))
            {
                parms.Add(new SqlParameter("Verb", actionCode));
            }
            if (nodeId.HasValue)
            {
                parms.Add(new SqlParameter("ClientId", nodeId.Value));
            }
            if (!string.IsNullOrEmpty(localEntityId))
            {
                parms.Add(new SqlParameter("LocalEntityId", localEntityId));
            }
            if (!string.IsNullOrEmpty(catalogCode))
            {
                parms.Add(new SqlParameter("CatalogCode", catalogCode + "%"));
            }

            var pArray = parms.ToArray();
            IList<MessageEntity> list = new List<MessageEntity>();
            using (var reader = this.GetCommandDb(ontology).ExecuteReader(queryString, pArray))
            {
                while (reader.Read())
                {
                    list.Add(CommandRecord.Create(ontology.Host, commandType, reader));
                }
            }
            total = (int)this.GetCommandDb(ontology).ExecuteScalar(countQS, parms.Select(p => ((ICloneable)p).Clone()).ToArray());

            return list;
        }
        #endregion

        #region GetCommand
        public MessageEntity GetCommand(MessageTypeKind commandType, OntologyDescriptor ontology, Guid id)
        {
            var queryString =
@"select * from " + GetTableName(commandType) + " as a where a.Id=@Id";
            using (var reader = this.GetCommandDb(ontology).ExecuteReader(queryString, new SqlParameter("Id", id)))
            {
                if (reader.Read())
                {
                    return CommandRecord.Create(ontology.Host, commandType, reader);
                }
            }

            return null;
        }
        #endregion

        #region private GetCommandDb
        /// <summary>
        /// 
        /// </summary>
        private RdbDescriptor GetCommandDb(OntologyDescriptor ontology)
        {
            if (!_dbDic.ContainsKey(ontology))
            {
                lock (_locker)
                {
                    if (!_dbDic.ContainsKey(ontology))
                    {
                        RdbDescriptor db;
                        if (!ontology.Host.Rdbs.TryDb(ontology.Ontology.MessageDatabaseId, out db))
                        {
                            throw new AnycmdException("意外的数据库Id" + ontology.Ontology.MessageDatabaseId.ToString());
                        }
                        _dbDic.Add(ontology, db);
                    }
                }
            }
            return _dbDic[ontology];
        }
        private string GetTableName(MessageTypeKind commandType)
        {
            switch (commandType)
            {
                case MessageTypeKind.Invalid:
                    throw new AnycmdException("意外的命令类型");
                case MessageTypeKind.AnyCommand:
                    throw new AnycmdException("AnyCommand不能持久化，没有TableName概念。");
                case MessageTypeKind.Received:
                    return "ReceivedMessage";
                case MessageTypeKind.Unaccepted:
                    return "UnacceptedMessage";
                case MessageTypeKind.Executed:
                    return "HandledCommand";
                case MessageTypeKind.ExecuteFailing:
                    return "HandleFailingCommand";
                case MessageTypeKind.Distribute:
                    return "DistributeMessage";
                case MessageTypeKind.Distributed:
                    return "DistributedMessage";
                case MessageTypeKind.DistributeFailing:
                    return "DistributeFailingMessage";
                case MessageTypeKind.LocalEvent:
                    return "LocalEvent";
                case MessageTypeKind.ClientEvent:
                    return "ClientEvent";
                default:
                    throw new AnycmdException("意外的命令类型");
            }
        }
        #endregion

        #region 内部类
        /// <summary>
        /// 命令记录。命令记录是命令实体。是<see cref="MessageProvider"/>所使用的命令模型。
        /// </summary>
        private sealed class CommandRecord : MessageEntity
        {
            #region Ctor
            /// <summary>
            /// 
            /// </summary>
            /// <param name="type"></param>
            /// <param name="dataTuple">数据项集合对</param>
            /// <param name="id">信息标识</param>
            public CommandRecord(MessageTypeKind type, Guid id, DataItemsTuple dataTuple)
                : base(type, id, dataTuple)
            {
            }
            #endregion

            /// <summary>
            /// 
            /// </summary>
            /// <param name="commandType"></param>
            /// <param name="record"></param>
            /// <returns></returns>
            internal static CommandRecord Create(IAcDomain host, MessageTypeKind commandType, System.Data.IDataRecord record)
            {
                MessageType requestType;
                record.GetString(record.GetOrdinal("MessageType")).TryParse(out requestType);
                ClientType clientType;
                record.GetString(record.GetOrdinal("ClientType")).TryParse(out clientType);
                return new CommandRecord(commandType,
                    record.GetGuid(record.GetOrdinal("Id")),
                    DataItemsTuple.Create(
                        host,
                        record.GetNullableString("InfoId"),
                        record.GetNullableString("InfoValue"),
                        record.GetNullableString("QueryList"),
                        record.GetString(record.GetOrdinal("InfoFormat"))))
                        {
                            Verb = new Verb(record.GetString(record.GetOrdinal("Verb"))),
                            Ontology = record.GetString(record.GetOrdinal("Ontology")),
                            LocalEntityId = record.GetNullableString("LocalEntityId"),
                            CatalogCode = record.GetNullableString("CatalogCode"),
                            ClientId = record.GetString(record.GetOrdinal("ClientId")),
                            TimeStamp = record.GetDateTime(record.GetOrdinal("TimeStamp")),
                            ReceivedOn = record.GetDateTime(record.GetOrdinal("ReceivedOn")),
                            CreateOn = record.GetDateTime(record.GetOrdinal("CreateOn")),
                            ClientType = clientType,
                            MessageType = requestType,
                            MessageId = record.GetString(record.GetOrdinal("MessageId")),
                            Status = record.GetInt32(record.GetOrdinal("Status")),
                            ReasonPhrase = record.GetNullableString(record.GetOrdinal("ReasonPhrase")),
                            Description = record.GetNullableString(record.GetOrdinal("Description")),
                            EventSubjectCode = record.GetNullableString(record.GetOrdinal("EventSubjectCode")),
                            EventSourceType = record.GetNullableString(record.GetOrdinal("EventSourceType")),
                            UserName = record.GetNullableString("UserName"),
                            IsDumb = record.GetBoolean(record.GetOrdinal("IsDumb")),
                            Version = record.GetString(record.GetOrdinal("Version"))
                        };
            }
        }
        #endregion
    }
}
