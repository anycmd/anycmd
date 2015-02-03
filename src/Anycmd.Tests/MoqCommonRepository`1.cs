
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
        private readonly IAcDomain _acDomain;

        public MoqCommonRepository(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
            _context = new MoqRepositoryContext(acDomain);
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
    public class MoqRepositoryContext : RepositoryContext
    {
        private readonly Guid _id = Guid.NewGuid();

        private static readonly Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>
            Data = new Dictionary<IAcDomain, Dictionary<Type, List<IAggregateRoot>>>();
        private readonly IAcDomain _acDomain;

        public MoqRepositoryContext(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
            if (!Data.ContainsKey(acDomain))
            {
                Data.Add(acDomain, new Dictionary<Type, List<IAggregateRoot>>());
            }
        }

        public override void Commit()
        {
            lock (_acDomain)
            {
                foreach (var item in base.NewCollection)
                {
                    if (!Data[_acDomain].ContainsKey(item.GetType()))
                    {
                        Data[_acDomain].Add(item.GetType(), new List<IAggregateRoot>());
                    }
                    if (Data[_acDomain][item.GetType()].Any(a => a.Id == ((IAggregateRoot)item).Id))
                    {
                        throw new Exception();
                    }
                    Data[_acDomain][item.GetType()].Add((IAggregateRoot)item);
                }
                foreach (var item in base.ModifiedCollection)
                {
                    Data[_acDomain][item.GetType()].Remove(Data[_acDomain][item.GetType()].First(a => a.Id == ((IAggregateRoot)item).Id));
                    Data[_acDomain][item.GetType()].Add((IAggregateRoot)item);
                }
                foreach (var item in DeletedCollection)
                {
                    Data[_acDomain][item.GetType()].Remove(Data[_acDomain][item.GetType()].First(a => a.Id == ((IAggregateRoot)item).Id));
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
            return !Data[_acDomain].ContainsKey(typeof(TEntity)) 
                ? new List<TEntity>().AsQueryable() : Data[_acDomain][typeof(TEntity)].Cast<TEntity>().AsQueryable<TEntity>();
        }
    }
}
