
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

    internal sealed class GroupSet : IGroupSet, IMemorySet
    {
        public static readonly IGroupSet Empty = new GroupSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, GroupState> _groupDic = new Dictionary<Guid, GroupState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;
        public Guid Id
        {
            get { return _id; }
        }

        internal GroupSet(IAcDomain host)
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

        public bool TryGetGroup(Guid groupId, out GroupState group)
        {
            if (!_initialized)
            {
                Init();
            }
            return _groupDic.TryGetValue(groupId, out group);
        }

        public IEnumerator<GroupState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _groupDic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _groupDic.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _groupDic.Clear();
                var groups = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllGroups();
                foreach (var group in groups)
                {
                    if (!_groupDic.ContainsKey(@group.Id))
                    {
                        _groupDic.Add(@group.Id, GroupState.Create(@group));
                    }
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddGroupCommand>,
            IHandler<AddPositionCommand>,
            IHandler<GroupAddedEvent>,
            IHandler<PositionAddedEvent>,
            IHandler<UpdateGroupCommand>,
            IHandler<UpdatePositionCommand>,
            IHandler<GroupUpdatedEvent>,
            IHandler<PositionUpdatedEvent>,
            IHandler<RemoveGroupCommand>, 
            IHandler<RemovePositionCommand>, 
            IHandler<GroupRemovedEvent>,
            IHandler<PositionRemovedEvent>,
            IHandler<OrganizationRemovedEvent>
        {
            private readonly GroupSet _set;

            internal MessageHandler(GroupSet set)
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
                messageDispatcher.Register((IHandler<AddGroupCommand>)this);
                messageDispatcher.Register((IHandler<GroupAddedEvent>)this);
                messageDispatcher.Register((IHandler<AddPositionCommand>)this);
                messageDispatcher.Register((IHandler<PositionAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateGroupCommand>)this);
                messageDispatcher.Register((IHandler<GroupUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<UpdatePositionCommand>)this);
                messageDispatcher.Register((IHandler<PositionUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveGroupCommand>)this);
                messageDispatcher.Register((IHandler<GroupRemovedEvent>)this);
                messageDispatcher.Register((IHandler<RemovePositionCommand>)this);
                messageDispatcher.Register((IHandler<PositionRemovedEvent>)this);
            }

            public void Handle(OrganizationRemovedEvent message)
            {
                var organizationCode = message.OrganizationCode;
                var groupIds = new HashSet<Guid>();
                foreach (var item in _set._groupDic.Values)
                {
                    if (!string.IsNullOrEmpty(item.OrganizationCode) && item.OrganizationCode.Equals(organizationCode, StringComparison.OrdinalIgnoreCase))
                    {
                        groupIds.Add(item.Id);
                    }
                }
                foreach (var groupId in groupIds)
                {
                    _set._host.Handle(new RemovePositionCommand(message.UserSession, groupId));
                }
            }

            public void Handle(AddGroupCommand message)
            {
                this.Handle(message.UserSession, message.Input, true);
            }

            public void Handle(GroupAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateGroupAddedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Output, false);
            }

            public void Handle(AddPositionCommand message)
            {
                this.Handle(message.UserSession, message.Input, true);
            }

            public void Handle(PositionAddedEvent message)
            {
                if (message.GetType() == typeof(PrivatePositionAddedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Output, false);
            }

            private void Handle(IUserSession userSession, IGroupCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var groupDic = _set._groupDic;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }

                var entity = Group.Create(input);

                lock (this)
                {
                    GroupState group;
                    if (host.GroupSet.TryGetGroup(entity.Id, out group))
                    {
                        throw new AnycmdException("意外的重复标识");
                    }
                    if (host.GroupSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ValidationException("重复的工作组名");
                    }
                    if (!groupDic.ContainsKey(entity.Id))
                    {
                        groupDic.Add(entity.Id, GroupState.Create(entity));
                    }
                    if (isCommand)
                    {
                        try
                        {
                            groupRepository.Add(entity);
                            groupRepository.Context.Commit();
                        }
                        catch
                        {
                            if (groupDic.ContainsKey(entity.Id))
                            {
                                groupDic.Remove(entity.Id);
                            }
                            groupRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateGroupAddedEvent(userSession, entity, input));
                }
            }

            private void Handle(IUserSession userSession, IPositionCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var groupDic = _set._groupDic;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (string.IsNullOrEmpty(input.OrganizationCode))
                {
                    throw new ValidationException("组织结构码不能为空");
                }
                OrganizationState org;
                if (!host.OrganizationSet.TryGetOrganization(input.OrganizationCode, out org))
                {
                    throw new ValidationException("非法的组织结构码" + input.OrganizationCode);
                }

                var entity = Group.Create(input);

                lock (this)
                {
                    GroupState group;
                    if (host.GroupSet.TryGetGroup(entity.Id, out group))
                    {
                        throw new AnycmdException("意外的重复标识");
                    }
                    if (host.GroupSet.Any(a => input.OrganizationCode.Equals(a.OrganizationCode, StringComparison.OrdinalIgnoreCase) && a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ValidationException("重复的岗位名");
                    }
                    if (!groupDic.ContainsKey(entity.Id))
                    {
                        groupDic.Add(entity.Id, GroupState.Create(entity));
                    }
                    if (isCommand)
                    {
                        try
                        {
                            groupRepository.Add(entity);
                            groupRepository.Context.Commit();
                        }
                        catch
                        {
                            if (groupDic.ContainsKey(entity.Id))
                            {
                                groupDic.Remove(entity.Id);
                            }
                            groupRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivatePositionAddedEvent(userSession, entity, input));
                }
            }

            private class PrivateGroupAddedEvent : GroupAddedEvent
            {
                internal PrivateGroupAddedEvent(IUserSession userSession, GroupBase source, IGroupCreateIo input)
                    : base(userSession, source, input)
                {

                }
            }
            private class PrivatePositionAddedEvent : PositionAddedEvent
            {
                internal PrivatePositionAddedEvent(IUserSession userSession, GroupBase source, IPositionCreateIo input)
                    : base(userSession, source, input)
                {

                }
            }

            public void Handle(UpdateGroupCommand message)
            {
                this.Handle(message.UserSession, message.Output, true);
            }

            public void Handle(GroupUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateGroupUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Output, false);
            }

            public void Handle(UpdatePositionCommand message)
            {
                this.Handle(message.UserSession, message.Output, true);
            }

            public void Handle(PositionUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivatePositionUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Output, false);
            }

            private void Handle(IUserSession userSession, IGroupUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                GroupState bkState;
                if (!host.GroupSet.TryGetGroup(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                Group entity;
                var stateChanged = false;
                lock (bkState)
                {
                    GroupState oldState;
                    if (!host.GroupSet.TryGetGroup(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (host.GroupSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
                    {
                        throw new ValidationException("重复的工作组名");
                    }
                    entity = groupRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = GroupState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            groupRepository.Update(entity);
                            groupRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            groupRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateGroupUpdatedEvent(userSession, entity, input));
                }
            }

            private void Handle(IUserSession userSession, IPositionUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                GroupState bkState;
                if (!host.GroupSet.TryGetGroup(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                if (string.IsNullOrEmpty(bkState.OrganizationCode))
                {
                    throw new AnycmdException("组织结构码为空");
                }
                Group entity;
                var stateChanged = false;
                lock (bkState)
                {
                    GroupState oldState;
                    if (!host.GroupSet.TryGetGroup(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (host.GroupSet.Any(a => bkState.OrganizationCode.Equals(a.OrganizationCode, StringComparison.OrdinalIgnoreCase) && a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
                    {
                        throw new ValidationException("重复的岗位名");
                    }
                    entity = groupRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = GroupState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            groupRepository.Update(entity);
                            groupRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            groupRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivatePositionUpdatedEvent(userSession, entity, input));
                }
            }

            private void Update(GroupState state)
            {
                var groupDic = _set._groupDic;
                groupDic[state.Id] = state;
            }

            private class PrivateGroupUpdatedEvent : GroupUpdatedEvent
            {
                internal PrivateGroupUpdatedEvent(IUserSession userSession, GroupBase source, IGroupUpdateIo input)
                    : base(userSession, source, input)
                {

                }
            }

            private class PrivatePositionUpdatedEvent : PositionUpdatedEvent
            {
                internal PrivatePositionUpdatedEvent(IUserSession userSession, GroupBase source, IPositionUpdateIo input)
                    : base(userSession, source, input)
                {

                }
            }

            public void Handle(RemoveGroupCommand message)
            {
                this.Handle(message.UserSession, message.EntityId, true);
            }

            public void Handle(GroupRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateGroupRemovedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Source.Id, false);
            }

            public void Handle(RemovePositionCommand message)
            {
                this.Handle(message.UserSession, message.EntityId, true);
            }

            public void Handle(PositionRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivatePositionRemovedEvent))
                {
                    return;
                }
                this.Handle(message.UserSession, message.Source.Id, false);
            }

            private void Handle(IUserSession userSession, Guid groupId, bool isCommand)
            {
                var host = _set._host;
                var groupDic = _set._groupDic;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                GroupState bkState;
                if (!host.GroupSet.TryGetGroup(groupId, out bkState))
                {
                    return;
                }
                Group entity;
                lock (bkState)
                {
                    GroupState state;
                    if (!host.GroupSet.TryGetGroup(groupId, out state))
                    {
                        return;
                    }
                    entity = groupRepository.GetByKey(groupId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (groupDic.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new GroupRemovingEvent(userSession, entity));
                        }
                        groupDic.Remove(bkState.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            groupRepository.Remove(entity);
                            groupRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!groupDic.ContainsKey(entity.Id))
                            {
                                groupDic.Add(bkState.Id, bkState);
                            }
                            groupRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateGroupRemovedEvent(userSession, entity));
                    if (!string.IsNullOrEmpty(bkState.OrganizationCode))
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivatePositionRemovedEvent(userSession, entity));
                    }
                }
            }

            private class PrivateGroupRemovedEvent : GroupRemovedEvent
            {
                internal PrivateGroupRemovedEvent(IUserSession userSession, GroupBase source)
                    : base(userSession, source)
                {

                }
            }

            private class PrivatePositionRemovedEvent : PositionRemovedEvent
            {
                internal PrivatePositionRemovedEvent(IUserSession userSession, GroupBase source)
                    : base(userSession, source)
                {

                }
            }
        }
        #endregion
    }
}
