
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Ssd;
    using Exceptions;
    using Host;
    using Rbac;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Util;

    internal sealed class SsdSetSet : ISsdSetSet, IMemorySet
    {
        public static readonly ISsdSetSet Empty = new SsdSetSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<Guid, SsdSetState> _ssdSetDic = new Dictionary<Guid, SsdSetState>();
        private readonly Dictionary<SsdSetState, List<SsdRoleState>> _ssdRoleBySet = new Dictionary<SsdSetState, List<SsdRoleState>>();
        private readonly Dictionary<Guid, SsdRoleState> _ssdRoleById = new Dictionary<Guid, SsdRoleState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;
        public Guid Id
        {
            get { return _id; }
        }

        internal SsdSetSet(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (acDomain.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            this._acDomain = acDomain;
            new MessageHandler(this).Register();
        }

        public bool TryGetSsdSet(Guid ssdSetId, out SsdSetState ssdSet)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(ssdSetId != Guid.Empty);

            return _ssdSetDic.TryGetValue(ssdSetId, out ssdSet);
        }

        public IReadOnlyCollection<SsdRoleState> GetSsdRoles(SsdSetState ssdSet)
        {
            if (!_initialized)
            {
                Init();
            }
            if (ssdSet == null)
            {
                throw new ArgumentNullException("ssdSet");
            }

            return !_ssdRoleBySet.ContainsKey(ssdSet) ? new List<SsdRoleState>() : _ssdRoleBySet[ssdSet];
        }

        public IReadOnlyCollection<SsdRoleState> GetSsdRoles()
        {
            if (!_initialized)
            {
                Init();
            }

            return _ssdRoleById.Select(item => item.Value).ToList();
        }

        public bool CheckRoles(HashSet<RoleState> roles, out string msg)
        {
            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }
            if (!_initialized)
            {
                Init();
            }
            foreach (var ssdSet in _ssdSetDic.Values)
            {
                var ssdRoles = _ssdRoleBySet[ssdSet];
                var ssdCard = ssdSet.SsdCard;
                if (roles.Count(a => ssdRoles.Any(b => b.RoleId == a.Id)) > ssdCard)
                {
                    msg = "违反了" + ssdSet.Name + "约束";
                    return false;
                }
            }
            msg = string.Empty;
            return true;
        }

        public IEnumerator<SsdSetState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _ssdSetDic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _ssdSetDic.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _ssdSetDic.Clear();
                _ssdRoleBySet.Clear();
                _ssdRoleById.Clear();
                var stateReder = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>();
                var ssdSets = stateReder.GetAllSsdSets();
                foreach (var ssdSet in ssdSets)
                {
                    if (!_ssdSetDic.ContainsKey(ssdSet.Id))
                    {
                        _ssdSetDic.Add(ssdSet.Id, SsdSetState.Create(ssdSet));
                    }
                }
                var ssdRoles = stateReder.GetAllSsdRoles();
                foreach (var ssdRole in ssdRoles)
                {
                    SsdSetState ssdSetState;
                    if (_ssdSetDic.TryGetValue(ssdRole.SsdSetId, out ssdSetState))
                    {
                        var state = SsdRoleState.Create(ssdRole);
                        if (!_ssdRoleById.ContainsKey(ssdRole.Id))
                        {
                            _ssdRoleById.Add(ssdRole.Id, state);
                        }
                        if (!_ssdRoleBySet.ContainsKey(ssdSetState))
                        {
                            _ssdRoleBySet.Add(ssdSetState, new List<SsdRoleState>());
                        }
                        _ssdRoleBySet[ssdSetState].Add(state);
                    }
                    else
                    {
                        // TODO:删除非法的记录
                    }
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddSsdSetCommand>,
            IHandler<SsdSetAddedEvent>,
            IHandler<SsdSetUpdatedEvent>,
            IHandler<UpdateSsdSetCommand>,
            IHandler<RemoveSsdSetCommand>,
            IHandler<SsdSetRemovedEvent>,
            IHandler<AddSsdRoleCommand>,
            IHandler<RemoveSsdRoleCommand>,
            IHandler<SsdRoleAddedEvent>,
            IHandler<SsdRoleRemovedEvent>
        {
            private readonly SsdSetSet _set;

            internal MessageHandler(SsdSetSet set)
            {
                this._set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._acDomain.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("AcDomain对象'{0}'尚未设置MessageDispatcher。".Fmt(_set._acDomain.Name));
                }
                messageDispatcher.Register((IHandler<AddSsdSetCommand>)this);
                messageDispatcher.Register((IHandler<SsdSetAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateSsdSetCommand>)this);
                messageDispatcher.Register((IHandler<SsdSetUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveSsdSetCommand>)this);
                messageDispatcher.Register((IHandler<SsdSetRemovedEvent>)this);
                messageDispatcher.Register((IHandler<AddSsdRoleCommand>)this);
                messageDispatcher.Register((IHandler<RemoveSsdRoleCommand>)this);
                messageDispatcher.Register((IHandler<SsdRoleAddedEvent>)this);
                messageDispatcher.Register((IHandler<SsdRoleRemovedEvent>)this);
            }

            public void Handle(AddSsdSetCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(SsdSetAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateSsdSetAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, ISsdSetCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ssdSetDic = _set._ssdSetDic;
                var ssdSetRepository = acDomain.RetrieveRequiredService<IRepository<SsdSet>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (acDomain.SsdSetSet.Any(a => a.Id == input.Id.Value))
                {
                    throw new AnycmdException("重复的SsdSet标识" + input.Id);
                }
                if (acDomain.SsdSetSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidationException("重复的静态责任分离角色集名称");
                }

                var entity = SsdSet.Create(input);

                lock (Locker)
                {
                    SsdSetState ssdSet;
                    if (acDomain.SsdSetSet.TryGetSsdSet(entity.Id, out ssdSet))
                    {
                        throw new AnycmdException("意外的重复标识");
                    }
                    if (!ssdSetDic.ContainsKey(entity.Id))
                    {
                        ssdSetDic.Add(entity.Id, SsdSetState.Create(entity));
                    }
                    if (isCommand)
                    {
                        try
                        {
                            ssdSetRepository.Add(entity);
                            ssdSetRepository.Context.Commit();
                        }
                        catch
                        {
                            if (ssdSetDic.ContainsKey(entity.Id))
                            {
                                ssdSetDic.Remove(entity.Id);
                            }
                            ssdSetRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateSsdSetAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateSsdSetAddedEvent : SsdSetAddedEvent, IPrivateEvent
            {
                internal PrivateSsdSetAddedEvent(IAcSession acSession, SsdSetBase source, ISsdSetCreateIo input)
                    : base(acSession, source, input)
                {
                }
            }

            public void Handle(UpdateSsdSetCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(SsdSetUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateSsdSetUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, ISsdSetUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ssdSetDic = _set._ssdSetDic;
                var ssdSetRepository = acDomain.RetrieveRequiredService<IRepository<SsdSet>>();
                SsdSetState bkState;
                if (!acDomain.SsdSetSet.TryGetSsdSet(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                SsdSet entity;
                var stateChanged = false;
                lock (Locker)
                {
                    SsdSetState oldState;
                    if (!acDomain.SsdSetSet.TryGetSsdSet(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (acDomain.SsdSetSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
                    {
                        throw new ValidationException("重复的静态责任分离角色组名");
                    }
                    entity = ssdSetRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = SsdSetState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            ssdSetRepository.Update(entity);
                            ssdSetRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            ssdSetRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateSsdSetUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(SsdSetState state)
            {
                var acDomain = _set._acDomain;
                var ssdSetDic = _set._ssdSetDic;
                ssdSetDic[state.Id] = state;
            }

            private class PrivateSsdSetUpdatedEvent : SsdSetUpdatedEvent, IPrivateEvent
            {
                internal PrivateSsdSetUpdatedEvent(IAcSession acSession, SsdSetBase source, ISsdSetUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveSsdSetCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(SsdSetRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateSsdSetRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid ssdSetId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ssdSetDic = _set._ssdSetDic;
                var ssdSetRepository = acDomain.RetrieveRequiredService<IRepository<SsdSet>>();
                SsdSetState bkState;
                if (!acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out bkState))
                {
                    return;
                }
                SsdSet entity;
                lock (Locker)
                {
                    SsdSetState state;
                    if (!acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out state))
                    {
                        return;
                    }
                    entity = ssdSetRepository.GetByKey(ssdSetId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (ssdSetDic.ContainsKey(bkState.Id))
                    {
                        ssdSetDic.Remove(bkState.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            ssdSetRepository.Remove(entity);
                            ssdSetRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!ssdSetDic.ContainsKey(entity.Id))
                            {
                                ssdSetDic.Add(bkState.Id, bkState);
                            }
                            ssdSetRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateSsdSetRemovedEvent(acSession, entity));
                }
            }

            private class PrivateSsdSetRemovedEvent : SsdSetRemovedEvent, IPrivateEvent
            {
                internal PrivateSsdSetRemovedEvent(IAcSession acSession, SsdSetBase source)
                    : base(acSession, source)
                {
                }
            }

            public void Handle(AddSsdRoleCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(SsdRoleAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateSsdRoleAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, ISsdRoleCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ssdRoleBySet = _set._ssdRoleBySet;
                var ssdRoleById = _set._ssdRoleById;
                var ssdRoleRepository = acDomain.RetrieveRequiredService<IRepository<SsdRole>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (ssdRoleById.Any(a => a.Key == input.Id.Value || (a.Value.RoleId == input.RoleId && a.Value.SsdSetId == input.SsdSetId)))
                {
                    throw new ValidationException("重复的记录");
                }
                SsdSetState ssdSet;
                if (!acDomain.SsdSetSet.TryGetSsdSet(input.SsdSetId, out ssdSet))
                {
                    throw new ValidationException("意外的静态责任分离角色集标识" + input.SsdSetId);
                }

                var entity = SsdRole.Create(input);

                lock (Locker)
                {
                    if (ssdRoleById.Any(a => a.Key == input.Id.Value || (a.Value.RoleId == input.RoleId && a.Value.SsdSetId == input.SsdSetId)))
                    {
                        throw new ValidationException("重复的记录");
                    }
                    if (!acDomain.SsdSetSet.TryGetSsdSet(input.SsdSetId, out ssdSet))
                    {
                        throw new ValidationException("意外的静态责任分离角色集标识" + input.SsdSetId);
                    }
                    var state = SsdRoleState.Create(entity);
                    if (!ssdRoleById.ContainsKey(entity.Id))
                    {
                        ssdRoleById.Add(entity.Id, state);
                    }
                    if (!ssdRoleBySet.ContainsKey(ssdSet))
                    {
                        ssdRoleBySet.Add(ssdSet, new List<SsdRoleState>());
                    }
                    ssdRoleBySet[ssdSet].Add(state);
                    if (isCommand)
                    {
                        try
                        {
                            ssdRoleRepository.Add(entity);
                            ssdRoleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (ssdRoleById.ContainsKey(entity.Id))
                            {
                                ssdRoleById.Remove(entity.Id);
                            }
                            ssdRoleBySet[ssdSet].Remove(state);
                            ssdRoleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateSsdRoleAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateSsdRoleAddedEvent : SsdRoleAddedEvent, IPrivateEvent
            {
                internal PrivateSsdRoleAddedEvent(IAcSession acSession, SsdRoleBase source, ISsdRoleCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveSsdRoleCommand message)
            {
                this.HandleSsdRole(message.AcSession, message.EntityId, true);
            }

            public void Handle(SsdRoleRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateSsdRoleRemovedEvent))
                {
                    return;
                }
                this.HandleSsdRole(message.AcSession, message.Source.Id, false);
            }

            private void HandleSsdRole(IAcSession acSession, Guid ssdRoleId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ssdSetDic = _set._ssdSetDic;
                var ssdRoleBySet = _set._ssdRoleBySet;
                var ssdRoleById = _set._ssdRoleById;
                var ssdRoleRepository = acDomain.RetrieveRequiredService<IRepository<SsdRole>>();
                SsdRoleState bkState;
                if (!ssdRoleById.TryGetValue(ssdRoleId, out bkState))
                {
                    return;
                }
                SsdRole entity;
                lock (Locker)
                {
                    SsdRoleState state;
                    if (!ssdRoleById.TryGetValue(ssdRoleId, out state))
                    {
                        return;
                    }
                    entity = ssdRoleRepository.GetByKey(ssdRoleId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (ssdRoleById.ContainsKey(bkState.Id))
                    {
                        ssdRoleById.Remove(bkState.Id);
                    }
                    SsdSetState ssdSet;
                    if (ssdSetDic.TryGetValue(entity.SsdSetId, out ssdSet))
                    {
                        ssdRoleBySet[ssdSet].Remove(bkState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            ssdRoleRepository.Remove(entity);
                            ssdRoleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!ssdRoleById.ContainsKey(entity.Id))
                            {
                                ssdRoleById.Add(bkState.Id, bkState);
                            }
                            if (ssdSetDic.TryGetValue(entity.SsdSetId, out ssdSet))
                            {
                                ssdRoleBySet[ssdSet].Add(bkState);
                            }
                            ssdRoleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateSsdRoleRemovedEvent(acSession, entity));
                }
            }

            private class PrivateSsdRoleRemovedEvent : SsdRoleRemovedEvent, IPrivateEvent
            {
                internal PrivateSsdRoleRemovedEvent(IAcSession acSession, SsdRoleBase source)
                    : base(acSession, source)
                {
                }
            }
        }
        #endregion
    }
}
