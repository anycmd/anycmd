
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
    using System.Diagnostics;
    using System.Linq;
    using Util;
    using roleId = System.Guid;

    internal sealed class RoleSet : IRoleSet, IMemorySet
    {
        public static readonly IRoleSet Empty = new RoleSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<roleId, RoleState> _roleDic = new Dictionary<roleId, RoleState>();
        private readonly Dictionary<RoleState, List<RoleState>> _descendantRoles = new Dictionary<RoleState, List<RoleState>>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        #region Ctor
        internal RoleSet(IAcDomain host)
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
        #endregion

        public bool TryGetRole(Guid roleId, out RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            return _roleDic.TryGetValue(roleId, out role);
        }

        public IReadOnlyCollection<RoleState> GetDescendantRoles(RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_descendantRoles.ContainsKey(role))
            {
                return new List<RoleState>();
            }
            return _descendantRoles[role];
        }

        public IReadOnlyCollection<RoleState> GetAscendantRoles(RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            var ancestors = new List<RoleState>();
            RecAncestorRoles(role, ancestors);
            
            return ancestors;
        }

        public IEnumerator<RoleState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _roleDic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _roleDic.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _roleDic.Clear();
                _descendantRoles.Clear();
                var roles = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllRoles();
                foreach (var role in roles)
                {
                    var roleState = RoleState.Create(role);
                    if (!_roleDic.ContainsKey(role.Id))
                    {
                        _roleDic.Add(role.Id, roleState);
                    }
                }
                foreach (var role in _roleDic)
                {
                    var children = new List<RoleState>();
                    RecDescendantRoles(this, role.Value, children);
                    _descendantRoles.Add(role.Value, children);
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }
        
        private void RecAncestorRoles(RoleState childRole, List<RoleState> ancestors)
        {
            foreach (var item in _host.PrivilegeSet.Where(a => a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Role))
            {
                if (item.ObjectInstanceId == childRole.Id)
                {
                    RoleState role;
                    if (_host.RoleSet.TryGetRole(item.SubjectInstanceId, out role))
                    {
                        RecAncestorRoles(role, ancestors);
                        ancestors.Add(role);
                    }
                }
            }
        }

        private static void RecDescendantRoles(RoleSet set, RoleState parentRole, List<RoleState> children)
        {
            var host = set._host;
            var roleDic = set._roleDic;
            foreach (var item in host.PrivilegeSet.Where(a => a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Role))
            {
                if (item.SubjectInstanceId == parentRole.Id)
                {
                    RoleState childRole;
                    if (roleDic.TryGetValue(item.ObjectInstanceId, out childRole))
                    {
                        RecDescendantRoles(set, childRole, children);
                        children.Add(childRole);
                    }
                }
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<UpdateRoleCommand>,
            IHandler<RoleUpdatedEvent>,
            IHandler<RoleRemovedEvent>,
            IHandler<AddRoleCommand>,
            IHandler<RoleAddedEvent>,
            IHandler<RemoveRoleCommand>,
            IHandler<RoleRolePrivilegeAddedEvent>,
            IHandler<RoleRolePrivilegeRemovedEvent>
        {
            private readonly RoleSet _set;

            internal MessageHandler(RoleSet set)
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
                messageDispatcher.Register((IHandler<AddRoleCommand>)this);
                messageDispatcher.Register((IHandler<RoleAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateRoleCommand>)this);
                messageDispatcher.Register((IHandler<RoleUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveRoleCommand>)this);
                messageDispatcher.Register((IHandler<RoleRemovedEvent>)this);
                messageDispatcher.Register((IHandler<RoleRolePrivilegeAddedEvent>)this);
                messageDispatcher.Register((IHandler<RoleRolePrivilegeRemovedEvent>)this);
            }

            public void Handle(AddRoleCommand message)
            {
                this.Handle(message.UserSession, message.Input, true);
            }

            public void Handle(RoleAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateRoleAddedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Output, false);
            }

            private void Handle(IUserSession userSession, IRoleCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var roleDic = _set._roleDic;
                var roleRepository = host.RetrieveRequiredService<IRepository<Role>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                Role entity;
                lock (this)
                {
                    RoleState role;
                    if (host.RoleSet.TryGetRole(input.Id.Value, out role))
                    {
                        throw new ValidationException("已经存在");
                    }
                    if (host.RoleSet.Any(a => string.Equals(a.Name, input.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ValidationException("同名的角色已经存在");
                    }

                    entity = Role.Create(input);

                    if (!roleDic.ContainsKey(entity.Id))
                    {
                        var state = RoleState.Create(entity);
                        roleDic.Add(entity.Id, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            roleRepository.Add(entity);
                            roleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (roleDic.ContainsKey(entity.Id))
                            {
                                roleDic.Remove(entity.Id);
                            }
                            roleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateRoleAddedEvent(userSession, entity, input));
                }
            }

            private class PrivateRoleAddedEvent : RoleAddedEvent
            {
                internal PrivateRoleAddedEvent(IUserSession userSession, RoleBase source, IRoleCreateIo input)
                    : base(userSession, source, input)
                {

                }
            }
            public void Handle(UpdateRoleCommand message)
            {
                this.Handle(message.UserSession, message.Output, true);
            }

            public void Handle(RoleUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateRoleUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Output, false);
            }

            private void Handle(IUserSession userSession, IRoleUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var roleDic = _set._roleDic;
                var roleRepository = host.RetrieveRequiredService<IRepository<Role>>();
                RoleState bkState;
                if (!host.RoleSet.TryGetRole(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                Role entity;
                bool stateChanged = false;
                lock (bkState)
                {
                    RoleState oldState;
                    if (!host.RoleSet.TryGetRole(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (host.RoleSet.Any(a => string.Equals(a.Name, input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
                    {
                        throw new ValidationException("角色名称重复");
                    }
                    entity = roleRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = RoleState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            roleRepository.Update(entity);
                            roleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            roleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateRoleUpdatedEvent(userSession, entity, input));
                }
            }

            private void Update(RoleState state)
            {
                var roleDic = _set._roleDic;
                roleDic[state.Id] = state;
            }

            private class PrivateRoleUpdatedEvent : RoleUpdatedEvent
            {
                internal PrivateRoleUpdatedEvent(IUserSession userSession, RoleBase source, IRoleUpdateIo input)
                    : base(userSession, source, input)
                {

                }
            }
            public void Handle(RemoveRoleCommand message)
            {
                this.Handle(message.UserSession, message.EntityId, true);
            }

            public void Handle(RoleRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateRoleRemovedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Source.Id, false);
            }

            private void Handle(IUserSession userSession, Guid roleId, bool isCommand)
            {
                var host = _set._host;
                var roleDic = _set._roleDic;
                var roleRepository = host.RetrieveRequiredService<IRepository<Role>>();
                RoleState bkState;
                if (!host.RoleSet.TryGetRole(roleId, out bkState))
                {
                    return;
                }
                Role entity;
                lock (bkState)
                {
                    RoleState state;
                    if (!host.RoleSet.TryGetRole(roleId, out state))
                    {
                        return;
                    }
                    entity = roleRepository.GetByKey(roleId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (roleDic.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new RoleRemovingEvent(userSession, entity));
                        }
                        roleDic.Remove(bkState.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            roleRepository.Remove(entity);
                            roleRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!roleDic.ContainsKey(bkState.Id))
                            {
                                roleDic.Add(bkState.Id, bkState);
                            }
                            roleRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateRoleRemovedEvent(userSession, entity));
                }
            }

            private class PrivateRoleRemovedEvent : RoleRemovedEvent
            {
                internal PrivateRoleRemovedEvent(IUserSession userSession, RoleBase source)
                    : base(userSession, source)
                {

                }
            }

            public void Handle(RoleRolePrivilegeAddedEvent message)
            {
                var roleDic = _set._roleDic;
                var descendantRoles = _set._descendantRoles;
                var entity = message.Source as PrivilegeBase;
                RoleState parentRole;
                Debug.Assert(entity != null, "entity != null");
                if (roleDic.TryGetValue(entity.SubjectInstanceId, out parentRole))
                {
                    lock (descendantRoles)
                    {
                        List<RoleState> value;
                        if (!descendantRoles.TryGetValue(parentRole, out value))
                        {
                            value = new List<RoleState>();
                            descendantRoles.Add(parentRole, value);
                        }
                        RoleState roleObject;
                        if (roleDic.TryGetValue(entity.ObjectInstanceId, out roleObject))
                        {
                            var children = new List<RoleState>();
                            RecDescendantRoles(_set, roleObject, children);
                            children.Add(roleObject);
                            foreach (var role in children)
                            {
                                if (value.All(a => a.Id != role.Id))
                                {
                                    value.Add(role);
                                }
                            }
                            var ancestorRoles = new List<RoleState>();
                            _set.RecAncestorRoles(parentRole, ancestorRoles);
                            foreach (var item in ancestorRoles)
                            {
                                if (!descendantRoles.TryGetValue(item, out value))
                                {
                                    value = new List<RoleState>();
                                    descendantRoles.Add(item, value);
                                }
                                foreach (var role in children)
                                {
                                    if (value.All(a => a.Id != role.Id))
                                    {
                                        value.Add(role);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public void Handle(RoleRolePrivilegeRemovedEvent message)
            {
                var roleDic = _set._roleDic;
                var descendantRoles = _set._descendantRoles;
                var entity = message.Source as PrivilegeBase;
                RoleState parentRole;
                Debug.Assert(entity != null, "entity != null");
                if (roleDic.TryGetValue(entity.SubjectInstanceId, out parentRole))
                {
                    lock (descendantRoles)
                    {
                        List<RoleState> value;
                        if (descendantRoles.TryGetValue(parentRole, out value))
                        {
                            if (roleDic.ContainsKey(entity.ObjectInstanceId))
                            {
                                descendantRoles[parentRole].Remove(roleDic[entity.ObjectInstanceId]);
                            }
                        }
                        RoleState roleObject;
                        if (roleDic.TryGetValue(entity.ObjectInstanceId, out roleObject))
                        {
                            var children = new List<RoleState>();
                            RecDescendantRoles(_set, roleObject, children);
                            children.Add(roleObject);
                            foreach (var role in children)
                            {
                                if (value.Any(a => a.Id == role.Id))
                                {
                                    value.Remove(role);
                                }
                            }
                            var ancestorRoles = new List<RoleState>();
                            _set.RecAncestorRoles(parentRole, ancestorRoles);
                            foreach (var item in ancestorRoles)
                            {
                                foreach (var role in children)
                                {
                                    if (value.Any(a => a.Id == role.Id))
                                    {
                                        value.Remove(role);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}