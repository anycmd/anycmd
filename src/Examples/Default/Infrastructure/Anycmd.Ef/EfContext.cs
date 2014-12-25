
namespace Anycmd.Ef
{
    using Exceptions;
    using System;
    using System.Configuration;

    /// <summary>
    /// Entity Framework实体框架上下文
    /// </summary>
    public static class EfContext
    {
        /// <summary>
        ///     An application-specific implementation of IObjectContextStorage must be setup either thru
        ///     <see cref = "InitStorage" /> or <see cref = "Storage" /> property.
        /// </summary>
        public static IEfContextStorage Storage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage"></param>
        public static void InitStorage(IEfContextStorage storage)
        {
            if (storage == null)
            {
                throw new System.ArgumentNullException("storage", @"storage mechanism was null but must be provided");
            }
            Storage = storage;
        }

        /// <summary>
        ///     Used to get the current NHibernate session associated with a factory key; i.e., the key 
        ///     associated with an NHibernate session factory for a specific database.
        /// </summary>
        public static System.Data.Entity.DbContext CreateDbContext(IAcDomain host, string efDbContextName)
        {
            if (string.IsNullOrEmpty(efDbContextName))
            {
                throw new ArgumentNullException(efDbContextName, @"objectContextKey may not be null or empty");
            }
            // alert:这里有个约定
            Rdb.RdbDescriptor db;
            string databaseKey = efDbContextName + "DatabaseId";
            Guid databaseId;
            if (!Guid.TryParse(ConfigurationManager.AppSettings[databaseKey], out databaseId))
            {
                throw new CoreException("DatabaseId应是Guid格式");
            }
            if (!host.Rdbs.TryDb(databaseId, out db))
            {
                throw new CoreException("意外的" + databaseKey + ":" + databaseId);
            }
            var efDbContext = new System.Data.Entity.DbContext(
                string.Format(ConfigurationManager.ConnectionStrings[efDbContextName].ConnectionString, db.ConnString));

            return efDbContext;
        }
    }
}