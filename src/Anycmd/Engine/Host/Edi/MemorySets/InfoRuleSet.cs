
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Engine.Edi;
    using Entities;
    using Info;
    using Model;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using Transactions;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class InfoRuleSet : IInfoRuleSet
    {
        public static readonly IInfoRuleSet Empty = new InfoRuleSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, InfoRuleState> _infoRuleEntities = new Dictionary<Guid, InfoRuleState>();

        private bool _initialized = false;
        private readonly object _locker = new object();
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 构造并接入总线
        /// </summary>
        internal InfoRuleSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            var messageDispatcher = host.MessageDispatcher;
            if (messageDispatcher == null)
            {
                throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(host.Name));
            }
        }

        public bool TryGetInfoRule(Guid id, out InfoRuleState infoRule)
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoRuleEntities.TryGetValue(id, out infoRule);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<InfoRuleState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoRuleEntities.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoRuleEntities.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (_locker)
            {
                if (_initialized) return;
                foreach (var item in _infoRuleEntities)
                {
                    item.Value.InfoRule.Dispose();
                }
                _infoRuleEntities.Clear();

                var infoRules = GetInfoRules();
                if (infoRules != null)
                {
                    // 填充信息项验证器库
                    foreach (var infoRule in infoRules.Where(a => a.IsEnabled == 1))
                    {
                        _infoRuleEntities.Add(infoRule.Id, infoRule);
                    }
                }

                _initialized = true;
            }
        }

        private IEnumerable<InfoRuleState> GetInfoRules()
        {
            IEnumerable<IInfoRule> validatorPlugs = null;
            using (var catalog = new DirectoryCatalog(Path.Combine(_host.GetPluginBaseDirectory(PluginType.InfoConstraint), "Bin")))
            {
                using (var container = new CompositionContainer(catalog))
                {
                    var infoRuleImport = new InfoRuleImport();
                    infoRuleImport.ImportsSatisfied += (sender, e) =>
                    {
                        validatorPlugs = e.InfoRules;
                    };
                    container.ComposeParts(infoRuleImport);
                }
            }

            var infoRuleRepository = _host.RetrieveRequiredService<IRepository<InfoRule>>();
            var oldEntities = infoRuleRepository.AsQueryable().ToList();
            var deleteList = new List<InfoRule>();
            var newList = new List<InfoRule>();
            var infoRules = new List<InfoRuleState>();
            var entities = new List<InfoRule>();
            bool saveChanges = false;
            foreach (var item in validatorPlugs)
            {
                var entity = new InfoRule
                {
                    Id = item.Id,
                    IsEnabled = 0
                };
                var oldEntity = oldEntities.FirstOrDefault(a => a.Id == item.Id);
                if (oldEntity != null)
                {
                    ((IEntityBase)entity).CreateBy = oldEntity.CreateBy;
                    ((IEntityBase)entity).CreateOn = oldEntity.CreateOn;
                    ((IEntityBase)entity).CreateUserId = oldEntity.CreateUserId;
                    entity.IsEnabled = oldEntity.IsEnabled;
                    ((IEntityBase)entity).ModifiedBy = oldEntity.ModifiedBy;
                    ((IEntityBase)entity).ModifiedOn = oldEntity.ModifiedOn;
                    ((IEntityBase)entity).ModifiedUserId = oldEntity.ModifiedUserId;
                }
                entities.Add(entity);
                infoRules.Add(InfoRuleState.Create(entity, item));
            }
            // 待添加的新的
            foreach (var item in entities)
            {
                var item1 = item;
                var old = oldEntities.FirstOrDefault(a => a.Id == item1.Id);
                if (old == null)
                {
                    newList.Add(item);
                }
            }
            // 待移除的旧的
            foreach (var oldEntity in oldEntities)
            {
                var item2 = oldEntity;
                var entity = entities.FirstOrDefault(a => a.Id == item2.Id);
                if (entity == null)
                {
                    deleteList.Add(oldEntity);
                }
            }
            if (newList.Count > 0)
            {
                saveChanges = true;
                foreach (var item in newList)
                {
                    infoRuleRepository.Context.RegisterNew(item);
                }
            }
            if (deleteList.Count > 0)
            {
                saveChanges = true;
                foreach (var item in deleteList)
                {
                    infoRuleRepository.Context.RegisterDeleted(item);
                }
            }
            if (saveChanges)
            {
                using (var coordinator = TransactionCoordinatorFactory.Create(infoRuleRepository.Context, _host.EventBus))
                {
                    coordinator.Commit();
                }
            }

            return infoRules;
        }

        private class InfoRuleImport : IPartImportsSatisfiedNotification
        {
            [ImportMany(typeof(IInfoRule), AllowRecomposition = true)]
            private IEnumerable<IInfoRule> InfoRules { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler<InfoRuleImportEventArgs> ImportsSatisfied;

            /// <summary>
            /// 在信息验证器部件导入并可安全使用时调用。
            /// </summary>
            public void OnImportsSatisfied()
            {
                if (ImportsSatisfied != null)
                {
                    ImportsSatisfied(this, new InfoRuleImportEventArgs(
                        this.InfoRules));
                }
            }
        }

        private class InfoRuleImportEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="infoRules"></param>
            public InfoRuleImportEventArgs(IEnumerable<IInfoRule> infoRules)
            {
                this.InfoRules = infoRules;
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<IInfoRule> InfoRules
            {
                get;
                private set;
            }
        }
    }
}
