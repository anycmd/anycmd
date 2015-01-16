
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Abstractions.Rbac;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;
    using Exceptions;
    using Host;
    using Rbac;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    internal sealed class DsdSetSet : IDsdSetSet, IMemorySet
    {
        public static readonly IDsdSetSet Empty = new DsdSetSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, DsdSetState> _dsdSetDic = new Dictionary<Guid, DsdSetState>();
        private readonly Dictionary<DsdSetState, List<DsdRoleState>> _dsdRoleBySet = new Dictionary<DsdSetState, List<DsdRoleState>>();
        private readonly Dictionary<Guid, DsdRoleState> _dsdRoleById = new Dictionary<Guid, DsdRoleState>();
        private bool _initialized;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;
        public Guid Id
        {
            get { return _id; }
        }

        internal DsdSetSet(IAcDomain host)
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

        public bool TryGetDsdSet(Guid dsdSetId, out DsdSetState dsdSet)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dsdSetDic.TryGetValue(dsdSetId, out dsdSet);
        }

        public IReadOnlyCollection<DsdRoleState> GetDsdRoles(DsdSetState dsdSet)
        {
            if (!_initialized)
            {
                Init();
            }
            if (dsdSet == null)
            {
                throw new ArgumentNullException("dsdSet");
            }
            return !_dsdRoleBySet.ContainsKey(dsdSet) ? new List<DsdRoleState>() : _dsdRoleBySet[dsdSet];
        }

        public IReadOnlyCollection<DsdRoleState> GetDsdRoles()
        {
            if (!_initialized)
            {
                Init();
            }

            return _dsdRoleById.Select(item => item.Value).ToList();
        }

        public bool CheckRoles(IEnumerable<RoleState> roles, out string msg)
        {
            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }
            if (!_initialized)
            {
                Init();
            }
            foreach (var dsdSet in _dsdSetDic.Values)
            {
                var dsdRoles = _dsdRoleBySet[dsdSet];
                var dsdCard = dsdSet.DsdCard;
                if (roles.Count(a => dsdRoles.Any(b => b.RoleId == a.Id)) > dsdCard)
                {
                    msg = "违反了" + dsdSet.Name + "约束";
                    return false;
                }
            }
            msg = string.Empty;
            return true;
        }

        public IEnumerator<DsdSetState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dsdSetDic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dsdSetDic.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dsdSetDic.Clear();
                _dsdRoleBySet.Clear();
                _dsdRoleById.Clear();
                var stateReder = _host.RetrieveRequiredService<IOriginalHostStateReader>();
                var dsdSets = stateReder.GetAllDsdSets();
                foreach (var dsdSet in dsdSets)
                {
                    if (!_dsdSetDic.ContainsKey(dsdSet.Id))
                    {
                        _dsdSetDic.Add(dsdSet.Id, DsdSetState.Create(dsdSet));
                    }
                }
                var dsdRoles = stateReder.GetAllDsdRoles();
                foreach (var dsdRole in dsdRoles)
                {
                    DsdSetState dsdSetState;
                    if (_dsdSetDic.TryGetValue(dsdRole.DsdSetId, out dsdSetState))
                    {
                        var state = DsdRoleState.Create(dsdRole);
                        if (!_dsdRoleById.ContainsKey(dsdRole.Id))
                        {
                            _dsdRoleById.Add(dsdRole.Id, state);
                        }
                        if (!_dsdRoleBySet.ContainsKey(dsdSetState))
                        {
                            _dsdRoleBySet.Add(dsdSetState, new List<DsdRoleState>());
                        }
                        _dsdRoleBySet[dsdSetState].Add(state);
                    }
                    else
                    {
                        // TODO:删除非法的记录
                    }
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddDsdSetCommand>,
            IHandler<DsdSetAddedEvent>,
            IHandler<DsdSetUpdatedEvent>,
            IHandler<UpdateDsdSetCommand>,
            IHandler<RemoveDsdSetCommand>,
            IHandler<DsdSetRemovedEvent>,
            IHandler<AddDsdRoleCommand>,
            IHandler<RemoveDsdRoleCommand>,
            IHandler<DsdRoleAddedEvent>,
            IHandler<DsdRoleRemovedEvent>
        {
            private readonly DsdSetSet _set;

            internal MessageHandler(DsdSetSet set)
            {
                _set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                }
                messageDispatcher.Register((IHandler<AddDsdSetCommand>)this);
                messageDispatcher.Register((IHandler<DsdSetAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateDsdSetCommand>)this);
                messageDispatcher.Register((IHandler<DsdSetUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveDsdSetCommand>)this);
                messageDispatcher.Register((IHandler<DsdSetRemovedEvent>)this);
                messageDispatcher.Register((IHandler<AddDsdRoleCommand>)this);
                messageDispatcher.Register((IHandler<RemoveDsdRoleCommand>)this);
                messageDispatcher.Register((IHandler<DsdRoleAddedEvent>)this);
                messageDispatcher.Register((IHandler<DsdRoleRemovedEvent>)this);
            }

            public void Handle(AddDsdSetCommand message)
            {
                Handle(message.Input, true);
            }

            public void Handle(DsdSetAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateDsdSetAddedEvent))
                {
                    return;
                }
                Handle(message.Output, false);
            }

            private void Handle(IDsdSetCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var dsdSetDic = _set._dsdSetDic;
                var dsdSetRepository = host.RetrieveRequiredService<IRepository<DsdSet>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (host.DsdSetSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidationException("重复的动态责任分离角色集名称");
                }

                var entity = DsdSet.Create(input);

                lock (this)
                {
                    DsdSetState dsdSet;
                    if (host.DsdSetSet.TryGetDsdSet(entity.Id, out dsdSet))
                    {
                        throw new AnycmdException("意外的重复标识");
                    }
                    if (!dsdSetDic.ContainsKey(entity.Id))
                    {
                        dsdSetDic.Add(entity.Id, DsdSetState.Create(entity));
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dsdSetRepository.Add(entity);
                            dsdSetRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dsdSetDic.ContainsKey(entity.Id))
                            {
                                dsdSetDic.Remove(entity.Id);
                            }
                            dsdSetRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDsdSetAddedEvent(entity, input));
                }
            }

            private class PrivateDsdSetAddedEvent : DsdSetAddedEvent
            {
                internal PrivateDsdSetAddedEvent(DsdSetBase source, IDsdSetCreateIo input)
                    : base(source, input)
                {
                }
            }

            public void Handle(UpdateDsdSetCommand message)
            {
                Handle(message.Output, true);
            }

            public void Handle(DsdSetUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateDsdSetUpdatedEvent))
                {
                    return;
                }
                Handle(message.Output, false);
            }

            private void Handle(IDsdSetUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var dsdSetRepository = host.RetrieveRequiredService<IRepository<DsdSet>>();
                DsdSetState bkState;
                if (!host.DsdSetSet.TryGetDsdSet(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                DsdSet entity;
                bool stateChanged;
                lock (bkState)
                {
                    DsdSetState oldState;
                    if (!host.DsdSetSet.TryGetDsdSet(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (host.DsdSetSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
                    {
                        throw new ValidationException("重复的静态责任分离角色组名");
                    }
                    entity = dsdSetRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = DsdSetState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dsdSetRepository.Update(entity);
                            dsdSetRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            dsdSetRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDsdSetUpdatedEvent(entity, input));
                }
            }

            private void Update(DsdSetState state)
            {
                var host = _set._host;
                var dsdSetDic = _set._dsdSetDic;
                var dsdRoleBySet = _set._dsdRoleBySet;
                var oldState = dsdSetDic[state.Id];
                dsdSetDic[state.Id] = state;
                if (dsdRoleBySet.ContainsKey(oldState) && !dsdRoleBySet.ContainsKey(state))
                {
                    dsdRoleBySet.Add(state, dsdRoleBySet[oldState]);
                    dsdRoleBySet.Remove(oldState);
                }
            }

            private class PrivateDsdSetUpdatedEvent : DsdSetUpdatedEvent
            {
                internal PrivateDsdSetUpdatedEvent(DsdSetBase source, IDsdSetUpdateIo input)
                    : base(source, input)
                {

                }
            }

            public void Handle(RemoveDsdSetCommand message)
            {
                Handle(message.EntityId, true);
            }

            public void Handle(DsdSetRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateDsdSetRemovedEvent))
                {
                    return;
                }
                Handle(message.Source.Id, false);
            }

            private void Handle(Guid dsdSetId, bool isCommand)
            {
                var host = _set._host;
                var dsdSetDic = _set._dsdSetDic;
                var dsdSetRepository = host.RetrieveRequiredService<IRepository<DsdSet>>();
                DsdSetState bkState;
                if (!host.DsdSetSet.TryGetDsdSet(dsdSetId, out bkState))
                {
                    return;
                }
                DsdSet entity;
                lock (bkState)
                {
                    DsdSetState state;
                    if (!host.DsdSetSet.TryGetDsdSet(dsdSetId, out state))
                    {
                        return;
                    }
                    entity = dsdSetRepository.GetByKey(dsdSetId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dsdSetDic.ContainsKey(bkState.Id))
                    {
                        dsdSetDic.Remove(bkState.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dsdSetRepository.Remove(entity);
                            dsdSetRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dsdSetDic.ContainsKey(entity.Id))
                            {
                                dsdSetDic.Add(bkState.Id, bkState);
                            }
                            dsdSetRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDsdSetRemovedEvent(entity));
                }
            }

            private class PrivateDsdSetRemovedEvent : DsdSetRemovedEvent
            {
                internal PrivateDsdSetRemovedEvent(DsdSetBase source)
                    : base(source)
                {
                }
            }

            public void Handle(AddDsdRoleCommand message)
            {
                Handle(message.Input, true);
            }

            public void Handle(DsdRoleAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateDsdRoleAddedEvent))
                {
                    return;
                }
                Handle(message.Output, false);
            }

            private void Handle(IDsdRoleCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var dsdRoleBySet = _set._dsdRoleBySet;
                var dsdRoleById = _set._dsdRoleById;
                var dsdRoleRepository = host.RetrieveRequiredService<IRepository<DsdRole>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (dsdRoleById.Any(a => a.Key == input.Id.Value || (a.Value.RoleId == input.RoleId && a.Value.DsdSetId == input.DsdSetId)))
                {
                    throw new ValidationException("重复的记录");
                }
                DsdSetState dsdSet;
                if (!host.DsdSetSet.TryGetDsdSet(input.DsdSetId, out dsdSet))
                {
                    throw new ValidationException("意外的动态责任分离角色集标识" + input.DsdSetId);
                }

                var entity = DsdRole.Create(input);

                lock (this)
                {
                    if (dsdRoleById.Any(a => a.Key == input.Id.Value || (a.Value.RoleId == input.RoleId && a.Value.DsdSetId == input.DsdSetId)))
                    {
                        throw new ValidationException("重复的记录");
                    }
                    if (!host.DsdSetSet.TryGetDsdSet(input.DsdSetId, out dsdSet))
                    {
                        throw new ValidationException("意外的动态责任分离角色集标识" + input.DsdSetId);
                    }
                    var state = DsdRoleState.Create(entity);
                    if (!dsdRoleById.ContainsKey(entity.Id))
                    {
                        dsdRoleById.Add(entity.Id, state);
                    }
                    if (!dsdRoleBySet.ContainsKey(dsdSet))
                    {
                        dsdRoleBySet.Add(dsdSet, new List<DsdRoleState>());
                    }
                    dsdRoleBySet[dsdSet].Add(state);
                    if (isCommand)
                    {
                        try
                        {
                            dsdRoleRepository.Add(entity);
                            dsdRoleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dsdRoleById.ContainsKey(entity.Id))
                            {
                                dsdRoleById.Remove(entity.Id);
                            }
                            dsdRoleBySet[dsdSet].Remove(state);
                            dsdRoleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDsdRoleAddedEvent(entity, input));
                }
            }

            private class PrivateDsdRoleAddedEvent : DsdRoleAddedEvent
            {
                internal PrivateDsdRoleAddedEvent(DsdRoleBase source, IDsdRoleCreateIo input)
                    : base(source, input)
                {

                }
            }

            public void Handle(RemoveDsdRoleCommand message)
            {
                HandleDsdRole(message.EntityId, true);
            }

            public void Handle(DsdRoleRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateDsdRoleRemovedEvent))
                {
                    return;
                }
                HandleDsdRole(message.Source.Id, false);
            }

            private void HandleDsdRole(Guid dsdRoleId, bool isCommand)
            {
                var host = _set._host;
                var dsdSetDic = _set._dsdSetDic;
                var dsdRoleBySet = _set._dsdRoleBySet;
                var dsdRoleById = _set._dsdRoleById;
                var dsdRoleRepository = host.RetrieveRequiredService<IRepository<DsdRole>>();
                DsdRoleState bkState;
                if (!dsdRoleById.TryGetValue(dsdRoleId, out bkState))
                {
                    return;
                }
                DsdRole entity;
                lock (bkState)
                {
                    DsdRoleState state;
                    if (!dsdRoleById.TryGetValue(dsdRoleId, out state))
                    {
                        return;
                    }
                    entity = dsdRoleRepository.GetByKey(dsdRoleId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dsdRoleById.ContainsKey(bkState.Id))
                    {
                        dsdRoleById.Remove(bkState.Id);
                    }
                    DsdSetState dsdSet;
                    if (dsdSetDic.TryGetValue(entity.DsdSetId, out dsdSet))
                    {
                        dsdRoleBySet[dsdSet].Remove(bkState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dsdRoleRepository.Remove(entity);
                            dsdRoleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dsdRoleById.ContainsKey(entity.Id))
                            {
                                dsdRoleById.Add(bkState.Id, bkState);
                            }
                            if (dsdSetDic.TryGetValue(entity.DsdSetId, out dsdSet))
                            {
                                dsdRoleBySet[dsdSet].Add(bkState);
                            }
                            dsdRoleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDsdRoleRemovedEvent(entity));
                }
            }

            private class PrivateDsdRoleRemovedEvent : DsdRoleRemovedEvent
            {
                internal PrivateDsdRoleRemovedEvent(DsdRoleBase source)
                    : base(source)
                {
                }
            }
        }
        #endregion
    }
}
