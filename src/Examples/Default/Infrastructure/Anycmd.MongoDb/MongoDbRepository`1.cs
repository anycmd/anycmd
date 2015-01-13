
namespace Anycmd.MongoDb
{
    using Model;
    using Repositories;
    using System;
    using System.Linq;

    public class MongoDbRepository<TAggregateRoot> : Repository<TAggregateRoot> 
        where TAggregateRoot : class, IAggregateRoot
    {
        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override TAggregateRoot DoGetByKey(ValueType key)
        {
            throw new NotImplementedException();
        }

        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override void DoRemove(TAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<TAggregateRoot> DoAsQueryable()
        {
            throw new NotImplementedException();
        }

        public override IRepositoryContext Context
        {
            get { throw new NotImplementedException(); }
        }
    }
}
