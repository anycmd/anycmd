
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
            IHandler<CatalogRemovedEvent>
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

            public void Handle(CatalogRemovedEvent message)
            {
                var catalogCode = message.CatalogCode;
                var groupIds = new HashSet<Guid>();
                foreach (var item in _set._groupDic.Values)
                {
                    if (!string.IsNullOrEmpty(item.CatalogCode) && item.CatalogCode.Equals(catalogCode, StringComparison.OrdinalIgnoreCase))
                    {
                        groupIds.Add(item.Id);
                    }
                }
                foreach (var groupId in groupIds)
                {
                    _set._host.Handle(new RemovePositionCommand(message.AcSession, groupId));
                }
            }

            public void Handle(AddGroupCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(GroupAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateGroupAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            public void Handle(AddPositionCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(PositionAddedEvent message)
            {
                if (message.GetType() == typeof(PrivatePositionAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IGroupCreateIo input, bool isCommand)
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
                    host.MessageDispatcher.DispatchMessage(new PrivateGroupAddedEvent(acSession, entity, input));
                }
            }

            private void Handle(IAcSession acSession, IPositionCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var groupDic = _set._groupDic;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (string.IsNullOrEmpty(input.CatalogCode))
                {
                    throw new ValidationException("目录码不能为空");
                }
                CatalogState org;
                if (!host.CatalogSet.TryGetCatalog(input.CatalogCode, out org))
                {
                    throw new ValidationException("非法的目录码" + input.CatalogCode);
                }

                var entity = Group.Create(input);

                lock (this)
                {
                    GroupState group;
                    if (host.GroupSet.TryGetGroup(entity.Id, out group))
                    {
                        throw new AnycmdException("意外的重复标识");
                    }
                    if (host.GroupSet.Any(a => input.CatalogCode.Equals(a.CatalogCode, StringComparison.OrdinalIgnoreCase) && a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
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
                    host.MessageDispatcher.DispatchMessage(new PrivatePositionAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateGroupAddedEvent : GroupAddedEvent
            {
                internal PrivateGroupAddedEvent(IAcSession acSession, GroupBase source, IGroupCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }
            private class PrivatePositionAddedEvent : PositionAddedEvent
            {
                internal PrivatePositionAddedEvent(IAcSession acSession, GroupBase source, IPositionCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(UpdateGroupCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(GroupUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateGroupUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            public void Handle(UpdatePositionCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(PositionUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivatePositionUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IGroupUpdateIo input, bool isCommand)
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
                    host.MessageDispatcher.DispatchMessage(new PrivateGroupUpdatedEvent(acSession, entity, input));
                }
            }

            private void Handle(IAcSession acSession, IPositionUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var groupRepository = host.RetrieveRequiredService<IRepository<Group>>();
                GroupState bkState;
                if (!host.GroupSet.TryGetGroup(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                if (string.IsNullOrEmpty(bkState.CatalogCode))
                {
                    throw new AnycmdException("目录码为空");
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
                    if (host.GroupSet.Any(a => bkState.CatalogCode.Equals(a.CatalogCode, StringComparison.OrdinalIgnoreCase) && a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
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
                    host.MessageDispatcher.DispatchMessage(new PrivatePositionUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(GroupState state)
            {
                var groupDic = _set._groupDic;
                groupDic[state.Id] = state;
            }

            private class PrivateGroupUpdatedEvent : GroupUpdatedEvent
            {
                internal PrivateGroupUpdatedEvent(IAcSession acSession, GroupBase source, IGroupUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            private class PrivatePositionUpdatedEvent : PositionUpdatedEvent
            {
                internal PrivatePositionUpdatedEvent(IAcSession acSession, GroupBase source, IPositionUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveGroupCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(GroupRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateGroupRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            public void Handle(RemovePositionCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(PositionRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivatePositionRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid groupId, bool isCommand)
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
                            host.MessageDispatcher.DispatchMessage(new GroupRemovingEvent(acSession, entity));
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
                    host.MessageDispatcher.DispatchMessage(new PrivateGroupRemovedEvent(acSession, entity));
                    if (!string.IsNullOrEmpty(bkState.CatalogCode))
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivatePositionRemovedEvent(acSession, entity));
                    }
                }
            }

            private class PrivateGroupRemovedEvent : GroupRemovedEvent
            {
                internal PrivateGroupRemovedEvent(IAcSession acSession, GroupBase source)
                    : base(acSession, source)
                {

                }
            }

            private class PrivatePositionRemovedEvent : PositionRemovedEvent
            {
                internal PrivatePositionRemovedEvent(IAcSession acSession, GroupBase source)
                    : base(acSession, source)
                {

                }
            }
        }
        #endregion
    }
}
