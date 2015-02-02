
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using Exceptions;
    using Host;
    using Infra;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    internal sealed class CatalogSet : ICatalogSet, IMemorySet
    {
        public static readonly ICatalogSet Empty = new CatalogSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<string, CatalogState> _dicByCode = new Dictionary<string, CatalogState>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Guid, CatalogState> _dicById = new Dictionary<Guid, CatalogState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        internal CatalogSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (host.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            this._host = host;
            new MessageHandler(this).Register();
        }

        public bool TryGetCatalog(Guid catalogId, out CatalogState oragnization)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.TryGetValue(catalogId, out oragnization);
        }

        public bool TryGetCatalog(string catalogCode, out CatalogState catalog)
        {
            if (!_initialized)
            {
                Init();
            }
            if (catalogCode == null)
            {
                catalog = CatalogState.Empty;
                return false;
            }
            return _dicByCode.TryGetValue(catalogCode, out catalog);
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dicByCode.Clear();
                _dicById.Clear();
                _dicByCode.Add(CatalogState.VirtualRoot.Code, CatalogState.VirtualRoot);
                _dicById.Add(CatalogState.VirtualRoot.Id, CatalogState.VirtualRoot);
                var allCatalogs = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetCatalogs().OrderBy(a => a.ParentCode);
                foreach (var catalog in allCatalogs)
                {
                    CatalogState orgState = CatalogState.Create(_host, catalog);
                    if (!_dicByCode.ContainsKey(catalog.Code))
                    {
                        _dicByCode.Add(catalog.Code, orgState);
                    }
                    if (!_dicById.ContainsKey(catalog.Id))
                    {
                        _dicById.Add(catalog.Id, orgState);
                    }
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        public IEnumerator<CatalogState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<UpdateCatalogCommand>, 
            IHandler<AddCatalogCommand>, 
            IHandler<CatalogAddedEvent>, 
            IHandler<CatalogUpdatedEvent>, 
            IHandler<RemoveCatalogCommand>, 
            IHandler<CatalogRemovedEvent>
        {
            private readonly CatalogSet _set;

            internal MessageHandler(CatalogSet set)
            {
                this._set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                }
                messageDispatcher.Register((IHandler<AddCatalogCommand>)this);
                messageDispatcher.Register((IHandler<CatalogAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateCatalogCommand>)this);
                messageDispatcher.Register((IHandler<CatalogUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveCatalogCommand>)this);
                messageDispatcher.Register((IHandler<CatalogRemovedEvent>)this);
            }

            public void Handle(AddCatalogCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(CatalogAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateCatalogAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, ICatalogCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var catalogRepository = host.RetrieveRequiredService<IRepository<Catalog>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new ValidationException("名称是必须的");
                }
                Catalog entity;
                lock (this)
                {
                    CatalogState catalog;
                    if (host.CatalogSet.TryGetCatalog(input.Id.Value, out catalog))
                    {
                        throw new ValidationException("给定标识的目录已经存在");
                    }
                    if (host.CatalogSet.TryGetCatalog(input.Code, out catalog))
                    {
                        throw new ValidationException("重复的目录码");
                    }
                    if (!string.IsNullOrEmpty(input.ParentCode))
                    {
                        CatalogState parentOragnization;
                        if (!host.CatalogSet.TryGetCatalog(input.ParentCode, out parentOragnization))
                        {
                            throw new NotExistException("标识为" + input.ParentCode + "的父目录不存在");
                        }
                        if (string.Equals(input.Code, input.ParentCode) || !input.Code.StartsWith(parentOragnization.Code, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException("子级目录的编码必须以父级目录编码为前缀");
                        }
                        if (host.CatalogSet.Any(a => string.Equals(a.ParentCode, input.ParentCode) && string.Equals(a.Name, input.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new ValidationException("兄弟目录间不能重名");
                        }
                    }
                    else
                    {
                        if (host.CatalogSet.Any(a => string.IsNullOrEmpty(a.ParentCode) && string.Equals(a.Name, input.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new ValidationException("重复的目录名");
                        }
                    }

                    entity = Catalog.Create(input);
                    var state = CatalogState.Create(host, entity);
                    if (!dicByCode.ContainsKey(entity.Code))
                    {
                        dicByCode.Add(entity.Code, state);
                    }
                    if (!dicById.ContainsKey(entity.Id))
                    {
                        dicById.Add(entity.Id, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            catalogRepository.Add(entity);
                            catalogRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dicByCode.ContainsKey(entity.Code))
                            {
                                dicByCode.Remove(entity.Code);
                            }
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            catalogRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateCatalogAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateCatalogAddedEvent : CatalogAddedEvent
            {
                internal PrivateCatalogAddedEvent(IAcSession acSession, CatalogBase source, ICatalogCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(UpdateCatalogCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(CatalogUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateCatalogUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Input, false);
            }

            private void Handle(IAcSession acSession, ICatalogUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var catalogRepository = host.RetrieveRequiredService<IRepository<Catalog>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                CatalogState bkState;
                if (!host.CatalogSet.TryGetCatalog(input.Id, out bkState))
                {
                    throw new ValidationException("给定标识的目录不存在" + input.Id);
                }
                Catalog entity;
                var stateChanged = false;
                lock (bkState)
                {
                    CatalogState oragnization;
                    if (host.CatalogSet.TryGetCatalog(input.Code, out oragnization) && oragnization.Id != input.Id)
                    {
                        throw new ValidationException("重复的目录码" + input.Code);
                    }
                    if (!string.IsNullOrEmpty(input.ParentCode))
                    {
                        CatalogState parentOragnization;
                        if (!host.CatalogSet.TryGetCatalog(input.ParentCode, out parentOragnization))
                        {
                            throw new NotExistException("标识为" + input.ParentCode + "的父目录不存在");
                        }
                        if (input.ParentCode.Equals(input.Code, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new AnycmdException("目录的父目录不能是自己");
                        }
                        if (!input.Code.StartsWith(parentOragnization.Code, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException("子级目录的编码必须以父级目录编码为前缀");
                        }
                        if (input.ParentCode.StartsWith(input.Code, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new AnycmdException("目录的父目录不能是自己的子孙级目录");
                        }
                    }
                    entity = catalogRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = CatalogState.Create(host, entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            catalogRepository.Update(entity);
                            catalogRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            catalogRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateCatalogUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(CatalogState state)
            {
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var oldKey = dicById[state.Id].Code;
                var newKey = state.Code;
                dicById[state.Id] = state;
                if (!dicByCode.ContainsKey(newKey))
                {
                    dicByCode.Add(newKey, state);
                    dicByCode.Remove(oldKey);
                }
                else
                {
                    dicByCode[newKey] = state;
                }
            }

            private class PrivateCatalogUpdatedEvent : CatalogUpdatedEvent
            {
                internal PrivateCatalogUpdatedEvent(IAcSession acSession, CatalogBase source, ICatalogUpdateIo input) : base(acSession, source, input) { }
            }

            public void Handle(RemoveCatalogCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(CatalogRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateCatalogRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid catalogId, bool isCommand)
            {
                var host = _set._host;
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var catalogRepository = host.RetrieveRequiredService<IRepository<Catalog>>();
                CatalogState bkState;
                if (!host.CatalogSet.TryGetCatalog(catalogId, out bkState))
                {
                    return;
                }
                Catalog entity;
                lock (bkState)
                {
                    CatalogState state;
                    if (!host.CatalogSet.TryGetCatalog(catalogId, out state))
                    {
                        return;
                    }
                    if (host.CatalogSet.Any(a => bkState.Code.Equals(a.ParentCode, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ValidationException("目录下有子目录时不能删除");
                    }
                    entity = catalogRepository.GetByKey(catalogId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new CatalogRemovingEvent(acSession, entity));
                        }
                        dicById.Remove(bkState.Id);
                        dicByCode.Remove(bkState.Code);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            catalogRepository.Remove(entity);
                            catalogRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dicById.ContainsKey(bkState.Id))
                            {
                                dicById.Add(bkState.Id, bkState);
                                dicByCode.Add(bkState.Code, bkState);
                            }
                            catalogRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateCatalogRemovedEvent(acSession, entity));
                }
            }

            private class PrivateCatalogRemovedEvent : CatalogRemovedEvent
            {
                internal PrivateCatalogRemovedEvent(IAcSession acSession, CatalogBase source)
                    : base(acSession, source)
                {

                }
            }
        }
        #endregion
    }
}