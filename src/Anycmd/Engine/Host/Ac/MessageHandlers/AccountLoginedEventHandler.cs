
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Bus;
    using Dapper;
    using Engine.Ac.Accounts;
    using Engine.Rdb;
    using Identity;
    using System;
    using System.Data;
    using System.Diagnostics;

    public class AccountLoginedEventHandler : IHandler<AccountLoginedEvent>
    {
        private readonly IAcDomain _acDomain;

        public AccountLoginedEventHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public void Handle(AccountLoginedEvent message)
        {
            var entity = message.Source as Account;
            Debug.Assert(entity != null, "entity != null");
            var dbId = new Guid("67E6CBF4-B481-4DDD-9FD9-1F0E06E9E1CB");
            RdbDescriptor db;
            if (!_acDomain.Rdbs.TryDb(dbId, out db))
            {
                //throw new CoreException("意外的数据库标识" + dbId);
                return;
            }
            using (var conn = db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("update [Account] set PreviousLoginOn=@PreviousLoginOn,FirstLoginOn=@FirstLoginOn,LoginCount=@LoginCount,IPAddress=@IPAddress where Id=@Id", new
                {
                    entity.Id,
                    entity.PreviousLoginOn,
                    entity.FirstLoginOn,
                    entity.LoginCount,
                    IPAddress = entity.IpAddress
                });
            }
        }
    }
}
