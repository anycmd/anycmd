
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Roles;
    using Engine.Ac.Abstractions.Rbac;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;
    using Engine.Ac.Privileges;
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
        private static readonly object Locker = new object();

        private readonly Dictionary<roleId, RoleState> _roleDic = new Dictionary<roleId, RoleState>();
        private readonly Dictionary<RoleState, List<RoleState>> _descendantRoles = new Dictionary<RoleState, List<RoleState>>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        #region Ctor
        internal RoleSet(IAcDomain acDomain)
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
        #endregion

        public bool TryGetRole(Guid roleId, out RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(roleId != Guid.Empty);

            return _roleDic.TryGetValue(roleId, out role);
        }

        public RoleState GetRole(Guid roleId)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(roleId != Guid.Empty);
            RoleState role;
            if (!_roleDic.TryGetValue(roleId, out role))
            {
                throw new NotExistException("给定的标识" + roleId + "标识的角色不存在");
            }

            return role;
        }

        public IReadOnlyCollection<RoleState> GetDescendantRoles(RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return !_descendantRoles.ContainsKey(role) ? new List<RoleState>() : _descendantRoles[role];
        }

        public IReadOnlyCollection<RoleState> GetAscendantRoles(RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            if (role == null)
            {
                throw new ArgumentNullException("role");
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
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _roleDic.Clear();
                _descendantRoles.Clear();
                var roles = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllRoles();
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
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        private void RecAncestorRoles(RoleState childRole, List<RoleState> ancestors)
        {
            foreach (var item in _acDomain.PrivilegeSet.Where(a => a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Role))
            {
                if (item.ObjectInstanceId == childRole.Id)
                {
                    RoleState role;
                    if (_acDomain.RoleSet.TryGetRole(item.SubjectInstanceId, out role))
                    {
                        RecAncestorRoles(role, ancestors);
                        ancestors.Add(role);
                    }
                }
            }
        }

        private static void RecDescendantRoles(RoleSet set, RoleState parentRole, List<RoleState> children)
        {
            var acDomain = set._acDomain;
            var roleDic = set._roleDic;
            foreach (var item in acDomain.PrivilegeSet.Where(a => a.SubjectType == AcElementType.Role && a.ObjectType == AcElementType.Role))
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
                var messageDispatcher = _set._acDomain.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("AcDomain对象'{0}'尚未设置MessageDispatcher。".Fmt(_set._acDomain.Name));
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
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(RoleAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateRoleAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IRoleCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var roleDic = _set._roleDic;
                var roleRepository = acDomain.RetrieveRequiredService<IRepository<Role>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                Role entity;
                lock (Locker)
                {
                    RoleState role;
                    if (acDomain.RoleSet.TryGetRole(input.Id.Value, out role))
                    {
                        throw new ValidationException("已经存在");
                    }
                    if (acDomain.RoleSet.Any(a => string.Equals(a.Name, input.Name, StringComparison.OrdinalIgnoreCase)))
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateRoleAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateRoleAddedEvent : RoleAddedEvent, IPrivateEvent
            {
                internal PrivateRoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(UpdateRoleCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(RoleUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateRoleUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IRoleUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var roleDic = _set._roleDic;
                var roleRepository = acDomain.RetrieveRequiredService<IRepository<Role>>();
                RoleState bkState;
                if (!acDomain.RoleSet.TryGetRole(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                Role entity;
                bool stateChanged = false;
                lock (Locker)
                {
                    RoleState oldState;
                    if (!acDomain.RoleSet.TryGetRole(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (acDomain.RoleSet.Any(a => string.Equals(a.Name, input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateRoleUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(RoleState state)
            {
                var roleDic = _set._roleDic;
                roleDic[state.Id] = state;
            }

            private class PrivateRoleUpdatedEvent : RoleUpdatedEvent, IPrivateEvent
            {
                internal PrivateRoleUpdatedEvent(IAcSession acSession, RoleBase source, IRoleUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveRoleCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(RoleRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateRoleRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid roleId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var roleDic = _set._roleDic;
                var roleRepository = acDomain.RetrieveRequiredService<IRepository<Role>>();
                RoleState bkState;
                if (!acDomain.RoleSet.TryGetRole(roleId, out bkState))
                {
                    return;
                }
                Role entity;
                lock (Locker)
                {
                    RoleState state;
                    if (!acDomain.RoleSet.TryGetRole(roleId, out state))
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
                            acDomain.MessageDispatcher.DispatchMessage(new RoleRemovingEvent(acSession, entity));
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateRoleRemovedEvent(acSession, entity));
                }
            }

            private class PrivateRoleRemovedEvent : RoleRemovedEvent, IPrivateEvent
            {
                internal PrivateRoleRemovedEvent(IAcSession acSession, RoleBase source)
                    : base(acSession, source)
                {

                }
            }

            public void Handle(RoleRolePrivilegeAddedEvent message)
            {
                var roleDic = _set._roleDic;
                var descendantRoles = _set._descendantRoles;
                var entity = message.Source as PrivilegeBase;
                Debug.Assert(entity != null, "entity != null");
                lock (Locker)
                {
                    RoleState parentRole;
                    if (!roleDic.TryGetValue(entity.SubjectInstanceId, out parentRole)) return;
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

            public void Handle(RoleRolePrivilegeRemovedEvent message)
            {
                var roleDic = _set._roleDic;
                var descendantRoles = _set._descendantRoles;
                var entity = message.Source as PrivilegeBase;
                Debug.Assert(entity != null, "entity != null");
                Debug.Assert(entity.SubjectInstanceId != Guid.Empty);
                lock (Locker)
                {
                    RoleState parentRole;
                    if (roleDic.TryGetValue(entity.SubjectInstanceId, out parentRole))
                    {
                        List<RoleState> value;
                        if (descendantRoles.TryGetValue(parentRole, out value))
                        {
                            if (roleDic.ContainsKey(entity.ObjectInstanceId))
                            {
                                descendantRoles[parentRole].Remove(roleDic[entity.ObjectInstanceId]);
                            }
                        }
                        else
                        {
                            throw new AnycmdException();
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