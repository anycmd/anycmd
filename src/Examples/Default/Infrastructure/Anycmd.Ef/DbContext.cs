using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Anycmd.Ef
{
    public class DbContext : System.Data.Entity.DbContext
    {
        protected DbContext() { }
        protected DbContext(DbCompiledModel model) : base(model) { }
        public DbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }
        public DbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext) { }
        public DbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }
        public DbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection) { }
    }
}
