
namespace Anycmd.MongoDb
{
    using Model;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using Repositories;
    using System;
    using System.Linq;

    public class MongoDbRepository<TAggregateRoot> : Repository<TAggregateRoot> 
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IMongoDbRepositoryContext _context;
        private readonly IAcDomain _acDomain;

        /// <summary>
        /// Initializes a new instance of <c>MongoDBRepository[TAggregateRoot]</c> class.
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="context">The <see cref="IRepositoryContext"/> object for initializing the current repository.</param>
        public MongoDbRepository(IAcDomain acDomain, IMongoDbRepositoryContext context)
        {
            if (acDomain == null)
                throw new ArgumentNullException("acDomain");
            if (context  == null)
                throw new ArgumentNullException("context");
            _acDomain = acDomain;
            _context = context;
        }

        public override IRepositoryContext Context
        {
            get { return _context; }
        }

        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            _context.RegisterNew(aggregateRoot);
        }

        protected override TAggregateRoot DoGetByKey(ValueType key)
        {
            MongoCollection collection = _context.GetCollectionForType(typeof(TAggregateRoot));
            var id = (Guid)key;

            return collection.AsQueryable<TAggregateRoot>().FirstOrDefault(p => p.Id == id);
        }

        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            _context.RegisterModified(aggregateRoot);
        }

        protected override void DoRemove(TAggregateRoot aggregateRoot)
        {
            _context.RegisterDeleted(aggregateRoot);
        }

        protected override IQueryable<TAggregateRoot> DoAsQueryable()
        {
            return _context.Query<TAggregateRoot>();
        }
    }
}
