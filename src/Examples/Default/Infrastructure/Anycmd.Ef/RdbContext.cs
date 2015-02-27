
namespace Anycmd.Ef
{
    using Engine.Rdb;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public class RdbContext : DbContext
    {
        private readonly RdbDescriptor _rdb;
        protected RdbContext(RdbDescriptor rdb)
            : base()
        {
            _rdb = rdb;
        }

        protected RdbContext(RdbDescriptor rdb, DbCompiledModel model) : base(model)
        {
            _rdb = rdb;
        }

        public RdbContext(RdbDescriptor rdb, string nameOrConnectionString) : base(nameOrConnectionString)
        {
            _rdb = rdb;
        }

        public RdbContext(RdbDescriptor rdb, DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            _rdb = rdb;
        }

        public RdbContext(RdbDescriptor rdb, ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            _rdb = rdb;
        }

        public RdbContext(RdbDescriptor rdb, string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            _rdb = rdb;
        }

        public RdbContext(RdbDescriptor rdb, DbConnection existingConnection, DbCompiledModel model,
            bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
            _rdb = rdb;
        }

        public RdbDescriptor Rdb
        {
            get { return _rdb; }
        }
    }
}
