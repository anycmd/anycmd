
namespace Anycmd.Ef
{
    using Engine.Host;
    using Exceptions;
    using Model;
    using Repositories;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class EfRepositoryContext : RepositoryContext, IEfRepositoryContext
    {
        private readonly string _efDbContextName;
        private DbContext _efContext;
        private readonly object _sync = new object();
        private readonly IAcDomain _host;

        public string EfDbContextName
        {
            get { return _efDbContextName; }
        }

        public EfRepositoryContext(IAcDomain host, string efDbContextName)
        {
            CheckConfig(efDbContextName);
            this._host = host;
            this._efDbContextName = efDbContextName;
        }

        private void CheckConfig(string efDbContextName)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[efDbContextName];
            if (connSetting == null || string.IsNullOrEmpty(connSetting.ConnectionString))
            {
                throw new Exceptions.AnycmdException("未配置name为" + efDbContextName + "的connectionStrings子节点");
            }
        }

        public DbContext DbContext
        {
            get { return _efContext ?? (_efContext = EfContext.CreateDbContext(_host, _efDbContextName)); }
        }

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // The dispose method will no longer be responsible for the commit
                // handling. Since the object container might handle the lifetime
                // of the repository context on the WCF per-request basis, users should
                // handle the commit logic by themselves.
                //if (!committed)
                //{
                //    Commit();
                //}
                if (_efContext != null)
                {
                    _efContext.Dispose();
                    _efContext = null;
                }
                base.Dispose(true);
            }
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterNew(object obj)
        {
            if (obj is IEntity)
            {
                if (((IEntity)obj).Id == Guid.Empty)
                {
                    throw new AnycmdException("实体标识不能为空");
                }
            }
            if ((obj is IEntityBase))
            {
                var entity = (obj as IEntityBase);
                if (entity.CreateUserId == null)
                {
                    var storage = _host.GetRequiredService<IUserSessionStorage>();
                    var user = storage.GetData(_host.Config.CurrentUserSessionCacheKey) as IUserSession;
                    if (user != null && user.Identity.IsAuthenticated)
                    {
                        if (string.IsNullOrEmpty(entity.CreateBy))
                        {
                            entity.CreateBy = user.Account.Name;
                        }
                        if (!entity.CreateUserId.HasValue)
                        {
                            entity.CreateUserId = user.Account.Id;
                        }
                    }
                    if (!entity.CreateOn.HasValue)
                    {
                        entity.CreateOn = DateTime.Now;
                    }
                }
            }
            this.DbContext.Entry(obj).State = EntityState.Added;
            Committed = false;
        }
        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterModified(object obj)
        {
            var state = this.DbContext.Entry(obj).State;
            if ((obj is IEntityBase) && state == EntityState.Modified)
            {
                var entity = (obj as IEntityBase);
                if (entity.ModifiedUserId == null)
                {
                    var storage = _host.GetRequiredService<IUserSessionStorage>();
                    var user = storage.GetData(_host.Config.CurrentUserSessionCacheKey) as IUserSession;
                    if (user != null && user.Identity.IsAuthenticated)
                    {
                        if (string.IsNullOrEmpty(entity.ModifiedBy))
                        {
                            entity.ModifiedBy = user.Account.Name;
                        }
                        if (!entity.ModifiedUserId.HasValue)
                        {
                            entity.ModifiedUserId = user.Account.Id;
                        }
                    }
                }
                entity.ModifiedOn = DateTime.Now;
            }
            Committed = false;
        }
        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterDeleted(object obj)
        {
            this.DbContext.Entry(obj).State = EntityState.Deleted;
            Committed = false;
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public override void Commit()
        {
            if (!Committed)
            {
                lock (_sync)
                {
                    ((IObjectContextAdapter)DbContext).ObjectContext.SaveChanges(SaveOptions.DetectChangesBeforeSave);
                }
                Committed = true;
            }
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            Committed = false;
        }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            return DbContext.Set<TEntity>();
        }
        #endregion
    }
}
