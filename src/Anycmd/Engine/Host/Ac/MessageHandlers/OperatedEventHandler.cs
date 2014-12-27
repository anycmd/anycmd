
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Anycmd.Rdb;
    using Events;
    using Exceptions;
    using Logging;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    /// <summary>
    /// 操作事件处理程序
    /// </summary>
    public class OperatedEventHandler : IDomainEventHandler<OperatedEvent>
    {
        readonly Guid _operationLogDbId = new Guid("67E6CBF4-B481-4DDD-9FD9-1F0E06E9E1CB");
        private readonly IAcDomain _host;

        public OperatedEventHandler(IAcDomain host)
        {
            this._host = host;
        }


        public void Handle(OperatedEvent evnt)
        {
            if (evnt == null)
            {
                return;
            }
            var log = evnt.Source as OperationLog;
            if (log == null) return;
            if (log.TargetId == Guid.Empty)
            {
                return;
            }
            RdbDescriptor db;
            if (!_host.Rdbs.TryDb(_operationLogDbId, out db))
            {
                throw new AnycmdException("意外的数据库标识");
            }
            // TODO:logbuffer
            const string sql = @"INSERT INTO dbo.OperationLog
                                    ( Id ,
                                      FunctionId ,
                                      AccountId ,
                                      EntityTypeId ,
                                      EntityTypeName ,
                                      AppSystemId ,
                                      AppSystemName ,
                                      ResourceTypeId ,
                                      ResourceName ,
                                      Description ,
                                      LoginName ,
                                      UserName ,
                                      CreateOn ,
                                      TargetId ,
                                      IPAddress
                                    )
                            VALUES  ( @Id ,
                                      @FunctionId ,
                                      @AccountId ,
                                      @EntityTypeId ,
                                      @EntityTypeName ,
                                      @AppSystemId ,
                                      @AppSystemName ,
                                      @ResourceTypeId ,
                                      @ResourceName ,
                                      @Description ,
                                      @LoginName ,
                                      @UserName ,
                                      @CreateOn ,
                                      @TargetId ,
                                      @IPAddress
                                    )";
            var ps = new List<SqlParameter>();
            if (log.Id == Guid.Empty)
            {
                log.Id = Guid.NewGuid();
            }
            ps.Add(new SqlParameter("Id", log.Id));
            ps.Add(new SqlParameter("FunctionId", log.FunctionId));
            ps.Add(new SqlParameter("AccountId", log.AccountId));
            ps.Add(new SqlParameter("EntityTypeId", log.EntityTypeId));
            ps.Add(log.EntityTypeName == null
                ? new SqlParameter("EntityTypeName", DBNull.Value)
                : new SqlParameter("EntityTypeName", log.EntityTypeName));
            ps.Add(new SqlParameter("AppSystemId", log.AppSystemId));
            ps.Add(log.AppSystemName == null
                ? new SqlParameter("AppSystemName", DBNull.Value)
                : new SqlParameter("AppSystemName", log.AppSystemName));
            ps.Add(new SqlParameter("ResourceTypeId", log.ResourceTypeId));
            ps.Add(log.ResourceName == null
                ? new SqlParameter("ResourceName", DBNull.Value)
                : new SqlParameter("ResourceName", log.ResourceName));
            ps.Add(log.Description == null
                ? new SqlParameter("Description", DBNull.Value)
                : new SqlParameter("Description", log.Description));
            ps.Add(log.LoginName == null
                ? new SqlParameter("LoginName", DBNull.Value)
                : new SqlParameter("LoginName", log.LoginName));
            ps.Add(log.UserName == null
                ? new SqlParameter("UserName", DBNull.Value)
                : new SqlParameter("UserName", log.UserName));
            ps.Add(new SqlParameter("CreateOn", log.CreateOn));
            ps.Add(new SqlParameter("TargetId", log.TargetId));
            ps.Add(log.IpAddress == null
                ? new SqlParameter("IPAddress", DBNull.Value)
                : new SqlParameter("IPAddress", log.IpAddress));
            db.ExecuteNonQuery(sql, ps.ToArray());
        }
    }
}
