
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Engine.Rdb;
    using Events;
    using Exceptions;
    using Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// 操作事件处理程序
    /// </summary>
    public class OperatedEventHandler : IDomainEventHandler<OperatedEvent>
    {
        readonly Guid _operationLogDbId = new Guid("67E6CBF4-B481-4DDD-9FD9-1F0E06E9E1CB");
        private readonly IAcDomain _acDomain;

        public OperatedEventHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
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
            if (!_acDomain.Rdbs.TryDb(_operationLogDbId, out db))
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
                                      IpAddress
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
                                      @IpAddress
                                    )";
            var ps = new List<DbParameter>();
            if (log.Id == Guid.Empty)
            {
                log.Id = Guid.NewGuid();
            }
            ps.Add(CreateParameter(db, "Id", log.Id, DbType.Guid));
            ps.Add(CreateParameter(db, "FunctionId", log.FunctionId, DbType.Guid));
            ps.Add(CreateParameter(db, "AccountId", log.AccountId, DbType.Guid));
            ps.Add(CreateParameter(db, "EntityTypeId", log.EntityTypeId, DbType.Guid));
            ps.Add(CreateParameter(db, "AppSystemId", log.AppSystemId, DbType.Guid));
            ps.Add(CreateParameter(db, "ResourceTypeId", log.ResourceTypeId, DbType.Guid));
            ps.Add(CreateParameter(db, "TargetId", log.TargetId, DbType.Guid));
            ps.Add(CreateParameter(db, "EntityTypeName", string.IsNullOrEmpty(log.EntityTypeName) ? DBNull.Value : (object)log.EntityTypeName, DbType.String));
            ps.Add(CreateParameter(db, "AppSystemName", string.IsNullOrEmpty(log.AppSystemName) ? DBNull.Value : (object)log.AppSystemName, DbType.String));
            ps.Add(CreateParameter(db, "ResourceName", string.IsNullOrEmpty(log.ResourceName) ? DBNull.Value : (object)log.ResourceName, DbType.String));
            ps.Add(CreateParameter(db, "Description", string.IsNullOrEmpty(log.Description) ? DBNull.Value : (object)log.Description, DbType.String));
            ps.Add(CreateParameter(db, "LoginName", string.IsNullOrEmpty(log.LoginName) ? DBNull.Value : (object)log.LoginName, DbType.String));
            ps.Add(CreateParameter(db, "UserName", string.IsNullOrEmpty(log.UserName) ? DBNull.Value : (object)log.UserName, DbType.String));
            ps.Add(CreateParameter(db, "IpAddress", string.IsNullOrEmpty(log.IpAddress) ? DBNull.Value : (object)log.IpAddress, DbType.String));
            ps.Add(CreateParameter(db, "CreateOn", log.CreateOn, DbType.DateTime));

            db.ExecuteNonQuery(sql, ps.ToArray());
        }

        private static DbParameter CreateParameter(RdbDescriptor db, string parameterName, object value, DbType dbType)
        {
            var p = db.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = value;
            p.DbType = dbType;

            return p;
        }
    }
}
