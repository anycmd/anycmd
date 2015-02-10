
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
    using System.Diagnostics;
    using System.Linq;
    using Util;

    internal sealed class GroupSet : IGroupSet, IMemorySet
    {
        public static readonly IGroupSet Empty = new GroupSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<Guid, GroupState> _groupDic = new Dictionary<Guid, GroupState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;
        public Guid Id
        {
            get { return _id; }
        }

        internal GroupSet(IAcDomain acDomain)
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

        public bool TryGetGroup(Guid groupId, out GroupState group)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(groupId != Guid.Empty);

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
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _groupDic.Clear();
                var groups = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllGroups();
                foreach (var group in groups)
                {
                    if (!_groupDic.ContainsKey(@group.Id))
                    {
                        _groupDic.Add(@group.Id, GroupState.Create(@group));
                    }
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddGroupCommand>,
            IHandler<GroupAddedEvent>,
            IHandler<UpdateGroupCommand>,
            IHandler<GroupUpdatedEvent>,
            IHandler<RemoveGroupCommand>, 
            IHandler<GroupRemovedEvent>
        {
            private readonly GroupSet _set;

            internal MessageHandler(GroupSet set)
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
                messageDispatcher.Register((IHandler<AddGroupCommand>)this);
                messageDispatcher.Register((IHandler<GroupAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateGroupCommand>)this);
                messageDispatcher.Register((IHandler<GroupUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveGroupCommand>)this);
                messageDispatcher.Register((IHandler<GroupRemovedEvent>)this);
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

            private void Handle(IAcSession acSession, IGroupCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var groupDic = _set._groupDic;
                var groupRepository = acDomain.RetrieveRequiredService<IRepository<Group>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }

                var entity = Group.Create(input);

                lock (Locker)
                {
                    GroupState group;
                    if (acDomain.GroupSet.TryGetGroup(entity.Id, out group))
                    {
                        throw new AnycmdException("意外的重复标识");
                    }
                    if (acDomain.GroupSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateGroupAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateGroupAddedEvent : GroupAddedEvent, IPrivateEvent
            {
                internal PrivateGroupAddedEvent(IAcSession acSession, GroupBase source, IGroupCreateIo input)
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

            private void Handle(IAcSession acSession, IGroupUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var groupRepository = acDomain.RetrieveRequiredService<IRepository<Group>>();
                GroupState bkState;
                if (!acDomain.GroupSet.TryGetGroup(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                Group entity;
                var stateChanged = false;
                lock (Locker)
                {
                    GroupState oldState;
                    if (!acDomain.GroupSet.TryGetGroup(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    if (acDomain.GroupSet.Any(a => a.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase) && a.Id != input.Id))
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateGroupUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(GroupState state)
            {
                var groupDic = _set._groupDic;
                groupDic[state.Id] = state;
            }

            private class PrivateGroupUpdatedEvent : GroupUpdatedEvent, IPrivateEvent
            {
                internal PrivateGroupUpdatedEvent(IAcSession acSession, GroupBase source, IGroupUpdateIo input)
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

            private void Handle(IAcSession acSession, Guid groupId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var groupDic = _set._groupDic;
                var groupRepository = acDomain.RetrieveRequiredService<IRepository<Group>>();
                GroupState bkState;
                if (!acDomain.GroupSet.TryGetGroup(groupId, out bkState))
                {
                    return;
                }
                Group entity;
                lock (Locker)
                {
                    GroupState state;
                    if (!acDomain.GroupSet.TryGetGroup(groupId, out state))
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
                            acDomain.MessageDispatcher.DispatchMessage(new GroupRemovingEvent(acSession, entity));
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateGroupRemovedEvent(acSession, entity));
                }
            }

            private class PrivateGroupRemovedEvent : GroupRemovedEvent, IPrivateEvent
            {
                internal PrivateGroupRemovedEvent(IAcSession acSession, GroupBase source)
                    : base(acSession, source)
                {

                }
            }
        }
        #endregion
    }
}
