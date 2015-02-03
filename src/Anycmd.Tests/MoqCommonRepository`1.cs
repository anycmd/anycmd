
namespace Anycmd.Tests
{
    using Model;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            lock (_host)
            {
                return Context.Query<TAggregateRoot>();
            }
        }

        public TAggregateRoot GetByKey(ValueType key)
        {
            lock (_host)
            {
                return Context.Query<TAggregateRoot>().FirstOrDefault(a => a.Id == (Guid)key);
            }
        }

        public void Add(TAggregateRoot aggregateRoot)
        {
            lock (_host)
            {
                Context.RegisterNew(aggregateRoot);
            }
        }

        public void Remove(TAggregateRoot aggregateRoot)
        {
            lock (_host)
            {
                Context.RegisterDeleted(aggregateRoot);
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            lock (_host)
            {
                Context.RegisterModified(aggregateRoot);
            }
        }
    }
    public class MoqRepositoryContext : RepositoryContext
    {
        private readonly Guid _id = Guid.NewGuid();

        private static readonly Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>
            Data = new Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>();
        private readonly IAcDomain _host;

        public MoqRepositoryContext(IAcDomain host)
        {
            this._host = host;
            if (!Data.ContainsKey(host))
            {
                Data.Add(host, new Dictionary<Type, List<IAggregateRoot>>());
            }
        }

        public override void Commit()
        {
            lock (_host)
            {
                foreach (var item in base.NewCollection)
                {
                    if (!Data[_host].ContainsKey(item.GetType()))
                    {
                        Data[_host].Add(item.GetType(), new List<IAggregateRoot>());
                    }
                    if (Data[_host][item.GetType()].Any(a => a.Id == ((IAggregateRoot)item).Id))
                    {
                        throw new Exception();
                    }
                    Data[_host][item.GetType()].Add((IAggregateRoot)item);
                }
                foreach (var item in base.ModifiedCollection)
                {
                    Data[_host][item.GetType()].Remove(Data[_host][item.GetType()].First(a => a.Id == ((IAggregateRoot)item).Id));
                    Data[_host][item.GetType()].Add((IAggregateRoot)item);
                }
                foreach (var item in DeletedCollection)
                {
                    Data[_host][item.GetType()].Remove(Data[_host][item.GetType()].First(a => a.Id == ((IAggregateRoot)item).Id));
                }
                base.Committed = true;
                base.ClearRegistrations();
            }
        }

        public override void Rollback()
        {
            lock (_host)
            {
                base.ClearRegistrations();
                base.Committed = false;
            }
        }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            lock (_host)
            {
                return !Data[_host].ContainsKey(typeof(TEntity)) ? new List<TEntity>().AsQueryable() : Data[_host][typeof(TEntity)].Cast<TEntity>().AsQueryable<TEntity>();
            }
        }
    }
}
