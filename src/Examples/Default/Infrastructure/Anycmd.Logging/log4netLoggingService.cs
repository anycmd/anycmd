
namespace Anycmd.Logging
{
    using Engine.Ac;
    using Exceptions;
    using log4net;
    using log4net.Config;
    using Query;
    using Rdb;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Util;

    /// <summary>
    /// <remarks>日志存储在引导库的AnyLog表</remarks>
    /// </summary>
    public sealed class Log4NetLoggingService : ILoggingService
    {
        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        private readonly string _bootConnString = ConfigurationManager.AppSettings["BootDbConnString"];

        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        public string BootConnString { get { return _bootConnString; } }

        private readonly IAcDomain _host;
        private readonly ILog _log;

        public Log4NetLoggingService(IAcDomain host)
        {

            log4net.GlobalContext.Properties["ProcessName"] = Process.GetCurrentProcess().ProcessName;
            log4net.GlobalContext.Properties["BaseDirectory"] = AppDomain.CurrentDomain.BaseDirectory;
            this._host = host;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            _log = LogManager.GetLogger(typeof(Log4NetLoggingService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anyLog"></param>
        public void Log(IAnyLog anyLog)
        {
            this.Log(new[] { anyLog });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anyLogs"></param>
        public void Log(IAnyLog[] anyLogs)
        {
            // 本组命令类型所对应的数据库表
            const string tableId = "[dbo][AnyLog]";
            RdbDescriptor db = GetAnyLogDb();
            DbTable dbTable;
            if (!db.TryGetDbTable(tableId, out dbTable))
            {
                throw new AnycmdException("意外的数据库表标识" + tableId);
            }
            // 当前命令表模式克隆得到的新表
            var dt = db.NewTable(dbTable);
            foreach (var log in anyLogs)
            {
                // 将当前命令转化DataRow，一个命令对应一行
                var row = log.ToDataRow(dt);
                dt.Rows.Add(row);
            }

            db.WriteToServer(dt);
        }

        public IAnyLog Get(Guid id)
        {
            RdbDescriptor db = GetAnyLogDb();
            string sql = "select * from AnyLog where Id=@Id";
            var reader = db.ExecuteReader(sql, new SqlParameter("Id", id));
            AnyLog anyLog = null;
            if (reader.Read())
            {
                anyLog = AnyLog.Create(reader);
            }
            reader.Close();
            return anyLog;
        }

        public IList<IAnyLog> GetPlistAnyLogs(List<FilterData> filters, PagingInput paging)
        {
            paging.Valid();
            var filterStringBuilder = _host.RetrieveRequiredService<ISqlFilterStringBuilder>();
            RdbDescriptor db = GetExceptionLogDb();
            List<SqlParameter> prams;
            var filterString = filterStringBuilder.FilterString(filters, "t", out prams);
            if (!string.IsNullOrEmpty(filterString))
            {
                filterString = " where " + filterString;
            }
            var sql =
@"select top({0}) * 
from (SELECT ROW_NUMBER() OVER(ORDER BY {1} {2}) AS RowNumber,* FROM {3} as t"
+ filterString + ") a WHERE a.RowNumber > {4}";
            var countSql = "select count(*) from AnyLog as t" + filterString;
            var anyLogs = new List<IAnyLog>();
            var reader = db.ExecuteReader(
                string.Format(sql, paging.PageSize, paging.SortField, paging.SortOrder, "AnyLog", paging.PageSize * paging.PageIndex), prams.ToArray());
            while (reader.Read())
            {
                anyLogs.Add(AnyLog.Create(reader));
            }
            paging.Total = (int)db.ExecuteScalar(countSql, prams.Select(p => ((ICloneable)p).Clone()).ToArray());
            reader.Close();

            return anyLogs;
        }

        public IList<OperationLog> GetPlistOperationLogs(Guid? targetId,
            DateTime? leftCreateOn, DateTime? rightCreateOn
            , List<FilterData> filters, PagingInput paging)
        {
            paging.Valid();
            var filterStringBuilder = _host.RetrieveRequiredService<ISqlFilterStringBuilder>();
            RdbDescriptor db = GetOperationLogDb();
            List<SqlParameter> prams;
            var filterString = filterStringBuilder.FilterString(filters, "t", out prams);
            if (!string.IsNullOrEmpty(filterString))
            {
                filterString = " where " + filterString + "{0}";
            }
            else
            {
                filterString = " where 1=1 {0}";
            }
            if (targetId.HasValue)
            {
                filterString = string.Format(filterString, " and t.TargetID=@TargetId {0}");
            }
            if (leftCreateOn.HasValue)
            {
                filterString = string.Format(filterString, " and t.CreateOn >= @leftCreateOn");
            }
            if (rightCreateOn.HasValue)
            {
                filterString = string.Format(filterString, " and t.CreateOn < @rightCreateOn");
            }
            else
            {
                filterString = string.Format(filterString, string.Empty);
            }
            var sql =
@"select top({0}) * 
from (SELECT ROW_NUMBER() OVER(ORDER BY {1} {2}) AS RowNumber,* FROM {3} as t"
+ filterString + ") a WHERE a.RowNumber > {4}";
            var countSql = "select count(*) from OperationLog as t" + filterString;

            var operationLogs = new List<OperationLog>();
            if (prams == null)
            {
                prams = new List<SqlParameter>();
            }
            if (targetId.HasValue)
            {
                prams.Add(new SqlParameter("TargetId", targetId.Value));
            }
            if (leftCreateOn.HasValue)
            {
                prams.Add(new SqlParameter("leftCreateOn", leftCreateOn.Value));
            }
            if (rightCreateOn.HasValue)
            {
                prams.Add(new SqlParameter("rightCreateOn", rightCreateOn.Value));
            }
            var reader = db.ExecuteReader(
                string.Format(sql, paging.PageSize, paging.SortField, paging.SortOrder, "OperationLog", paging.PageSize * paging.PageIndex), prams.ToArray());
            while (reader.Read())
            {
                operationLogs.Add(OperationLog.Create(reader));
            }
            paging.Total = (int)db.ExecuteScalar(countSql, prams.Select(p => ((ICloneable)p).Clone()).ToArray());
            reader.Close();

            return operationLogs;
        }

        public IList<ExceptionLog> GetPlistExceptionLogs(List<FilterData> filters, PagingInput paging)
        {
            paging.Valid();
            var filterStringBuilder = _host.RetrieveRequiredService<ISqlFilterStringBuilder>();
            RdbDescriptor db = GetExceptionLogDb();
            List<SqlParameter> prams;
            var filterString = filterStringBuilder.FilterString(filters, "t", out prams);
            if (!string.IsNullOrEmpty(filterString))
            {
                filterString = " where " + filterString;
            }
            var sql =
@"select top({0}) * 
from (SELECT ROW_NUMBER() OVER(ORDER BY {1} {2}) AS RowNumber,* FROM {3} as t"
+ filterString + ") a WHERE a.RowNumber > {4}";
            var countSql = "select count(*) from ExceptionLog as t" + filterString;
            var exceptionLogs = new List<ExceptionLog>();
            var reader = db.ExecuteReader(
                string.Format(sql, paging.PageSize, paging.SortField, paging.SortOrder, "ExceptionLog", paging.PageSize * paging.PageIndex), prams.ToArray());
            while (reader.Read())
            {
                exceptionLogs.Add(ExceptionLog.Create(reader));
            }
            paging.Total = (int)db.ExecuteScalar(countSql, prams.Select(p => ((ICloneable)p).Clone()).ToArray());
            reader.Close();

            return exceptionLogs;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAnyLog()
        {
            const string sql = "TRUNCATE TABLE dbo.AnyLog";
            var db = GetAnyLogDb();
            db.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearExceptionLog()
        {
            const string sql = "TRUNCATE TABLE dbo.ExceptionLog";
            var db = GetExceptionLogDb();
            db.ExecuteNonQuery(sql);
        }

        private RdbDescriptor GetAnyLogDb()
        {

            EntityTypeState entityType;
            if (!_host.EntityTypeSet.TryGetEntityType(new Coder("Ac", "AnyLog"), out entityType))
            {
                throw new Exceptions.AnycmdException("意外的实体类型码Ac.AnyLog");
            }
            RdbDescriptor db;
            if (!_host.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new Exceptions.AnycmdException("意外的AnyLog数据库标识" + entityType.DatabaseId);
            }
            return db;
        }

        private RdbDescriptor GetOperationLogDb()
        {

            EntityTypeState entityType;
            if (!_host.EntityTypeSet.TryGetEntityType(new Coder("Ac", "OperationLog"), out entityType))
            {
                throw new Exceptions.AnycmdException("意外的实体类型码Ac.OperationLog");
            }
            RdbDescriptor db;
            if (!_host.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new Exceptions.AnycmdException("意外的OperationLog数据库标识" + entityType.DatabaseId);
            }
            return db;
        }

        private RdbDescriptor GetExceptionLogDb()
        {

            EntityTypeState entityType;
            if (!_host.EntityTypeSet.TryGetEntityType(new Coder("Ac", "ExceptionLog"), out entityType))
            {
                throw new Exceptions.AnycmdException("意外的实体类型码Ac.ExceptionLog");
            }
            RdbDescriptor db;
            if (!_host.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new Exceptions.AnycmdException("意外的ExceptionLog数据库标识" + entityType.DatabaseId);
            }
            return db;
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void DebugFormatted(string format, params object[] args)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void InfoFormatted(string format, params object[] args)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void WarnFormatted(string format, params object[] args)
        {
            _log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void ErrorFormatted(string format, params object[] args)
        {
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void FatalFormatted(string format, params object[] args)
        {
            _log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }

        public bool IsDebugEnabled
        {
            get
            {
                return _log.IsDebugEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return _log.IsInfoEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return _log.IsWarnEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return _log.IsErrorEnabled;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return _log.IsFatalEnabled;
            }
        }
    }
}
