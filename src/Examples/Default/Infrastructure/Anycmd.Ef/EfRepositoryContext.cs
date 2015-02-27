
namespace Anycmd.Ef
{
    using Engine.Host;
    using Exceptions;
    using Model;
    using Repositories;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class EfRepositoryContext : RepositoryContext, IEfRepositoryContext
    {
        private readonly string _efDbContextName;
        private RdbContext _efContext;
        private readonly object _sync = new object();
        private readonly IAcDomain _acDomain;

        public string EfDbContextName
        {
            get { return _efDbContextName; }
        }

        public EfRepositoryContext(IAcDomain acDomain, string efDbContextName)
        {
            CheckConfig(efDbContextName);
            this._acDomain = acDomain;
            this._efDbContextName = efDbContextName;
        }

        private static void CheckConfig(string efDbContextName)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[efDbContextName];
            if (connSetting == null || string.IsNullOrEmpty(connSetting.ConnectionString))
            {
                throw new AnycmdException("未配置name为" + efDbContextName + "的connectionStrings子节点");
            }
        }

        public RdbContext DbContext
        {
            get { return _efContext ?? (_efContext = EfContext.CreateDbContext(_acDomain, _efDbContextName)); }
        }

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_efContext != null)
            {
                _efContext.Dispose();
                _efContext = null;
            }
            base.Dispose(true);
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterNew(object obj)
        {
            var entity = obj as IEntity;
            if (entity != null)
            {
                if (entity.Id == Guid.Empty)
                {
                    throw new AnycmdException("实体标识不能为空");
                }
            }
            var entityBase = obj as IEntityBase;
            if (entityBase != null)
            {
                if (entityBase.CreateUserId == null)
                {
                    var storage = _acDomain.GetRequiredService<IAcSessionStorage>();
                    var user = storage.GetData(_acDomain.Config.CurrentAcSessionCacheKey) as IAcSession;
                    if (user != null && user.Identity.IsAuthenticated)
                    {
                        if (string.IsNullOrEmpty(entityBase.CreateBy))
                        {
                            entityBase.CreateBy = user.Account.Name;
                        }
                        if (!entityBase.CreateUserId.HasValue)
                        {
                            entityBase.CreateUserId = user.Account.Id;
                        }
                    }
                    if (!entityBase.CreateOn.HasValue)
                    {
                        entityBase.CreateOn = DateTime.Now;
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
            var entityBase = obj as IEntityBase;
            if (entityBase != null && state == EntityState.Modified)
            {
                if (entityBase.ModifiedUserId == null)
                {
                    var storage = _acDomain.GetRequiredService<IAcSessionStorage>();
                    var user = storage.GetData(_acDomain.Config.CurrentAcSessionCacheKey) as IAcSession;
                    if (user != null && user.Identity.IsAuthenticated)
                    {
                        if (string.IsNullOrEmpty(entityBase.ModifiedBy))
                        {
                            entityBase.ModifiedBy = user.Account.Name;
                        }
                        if (!entityBase.ModifiedUserId.HasValue)
                        {
                            entityBase.ModifiedUserId = user.Account.Id;
                        }
                    }
                }
                entityBase.ModifiedOn = DateTime.Now;
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
            if (Committed) return;
            lock (_sync)
            {
                try
                {
                    DbContext.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var msg = string.Empty;

                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                        foreach (var validationError in validationErrors.ValidationErrors)
                            msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                    var fail = new AnycmdException(msg, dbEx);
                    //Debug.WriteLine(fail.Message, fail);
                    throw fail;
                }
            }
            Committed = true;
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
