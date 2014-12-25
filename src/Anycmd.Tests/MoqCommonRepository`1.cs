
namespace Anycmd.Tests
{
    using Model;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class MoqCommonRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly MoqRepositoryContext _context;
        private readonly IAcDomain _host;

        public MoqCommonRepository(IAcDomain host)
        {
            this._host = host;
            _context = new MoqRepositoryContext(host);
        }

        public IRepositoryContext Context
        {
            get { return _context; }
        }

        public IQueryable<TAggregateRoot> AsQueryable()
        {
            return Context.Query<TAggregateRoot>();
        }

        public TAggregateRoot GetByKey(ValueType key)
        {
            return Context.Query<TAggregateRoot>().FirstOrDefault(a => a.Id == (Guid)key);
        }

        public void Add(TAggregateRoot aggregateRoot)
        {
            Context.RegisterNew(aggregateRoot);
        }

        public void Remove(TAggregateRoot aggregateRoot)
        {
            Context.RegisterDeleted(aggregateRoot);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            Context.RegisterModified(aggregateRoot);
        }
    }
    public class MoqRepositoryContext : RepositoryContext, IRepositoryContext
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly object _sync = new object();

        private static readonly ThreadLocal<Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>> 
            Data = new ThreadLocal<Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>>(() => new Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>());
        private readonly IAcDomain _host;

        public MoqRepositoryContext(IAcDomain host)
        {
            this._host = host;
            if (!Data.Value.ContainsKey(host))
            {
                Data.Value.Add(host, new Dictionary<Type, List<IAggregateRoot>>());
            }
        }

        public override void Commit()
        {
            lock (_sync)
            {
                foreach (var item in base.NewCollection)
                {
                    if (!Data.Value[_host].ContainsKey(item.GetType()))
                    {
                        Data.Value[_host].Add(item.GetType(), new List<IAggregateRoot>());
                    }
                    if (Data.Value[_host][item.GetType()].Any(a => a.Id == ((IAggregateRoot)item).Id))
                    {
                        throw new Exception();
                    }
                    Data.Value[_host][item.GetType()].Add((IAggregateRoot)item);
                }
                foreach (var item in base.ModifiedCollection)
                {
                    Data.Value[_host][item.GetType()].Remove(Data.Value[_host][item.GetType()].First(a => a.Id == ((IAggregateRoot)item).Id));
                    Data.Value[_host][item.GetType()].Add((IAggregateRoot)item);
                }
                foreach (var item in DeletedCollection)
                {
                    Data.Value[_host][item.GetType()].Remove(Data.Value[_host][item.GetType()].First(a => a.Id == ((IAggregateRoot)item).Id));
                }
                base.Committed = true;
                base.ClearRegistrations();
            }
        }

        public override void Rollback()
        {
            base.ClearRegistrations();
            base.Committed = false;
        }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            return !Data.Value[_host].ContainsKey(typeof(TEntity)) ? new List<TEntity>().AsQueryable() : Data.Value[_host][typeof(TEntity)].Cast<TEntity>().AsQueryable<TEntity>();
        }
    }
}
