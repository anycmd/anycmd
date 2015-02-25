
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Accounts;
    using Exceptions;
    using Host;
    using Identity;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Util;
    using loginName = System.String;

    internal sealed class SysUserSet : ISysUserSet, IMemorySet
    {
        public static readonly ISysUserSet Empty = new SysUserSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<Guid, AccountState> _devAccountById = new Dictionary<Guid, AccountState>();
        private readonly Dictionary<loginName, AccountState> _devAccountByLoginName = new Dictionary<loginName, AccountState>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        internal SysUserSet(IAcDomain acDomain)
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
            new MessageHandle(this).Register();
        }

        public IReadOnlyCollection<AccountState> GetDevAccounts()
        {
            if (!_initialized)
            {
                Init();
            }
            // 不存储就要计算，存储就会占内存
            return new List<AccountState>(_devAccountById.Values);
        }

        public bool TryGetDevAccount(string developerCode, out AccountState developer)
        {
            if (!_initialized)
            {
                Init();
            }
            if (string.IsNullOrEmpty(developerCode))
            {
                throw new ArgumentNullException("developerCode");
            }

            return _devAccountByLoginName.TryGetValue(developerCode, out developer);
        }

        public bool TryGetDevAccount(Guid accountId, out AccountState developer)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(accountId != Guid.Empty);

            return _devAccountById.TryGetValue(accountId, out developer);
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        public IEnumerator<AccountState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _devAccountById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _devAccountById.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _devAccountById.Clear();
                _devAccountByLoginName.Clear();
                var accounts = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllDevAccounts();
                foreach (var account in accounts)
                {
                    var accountState = AccountState.Create(account);
                    if (!_devAccountById.ContainsKey(account.Id))
                    {
                        _devAccountById.Add(account.Id, accountState);
                    }
                    if (!_devAccountByLoginName.ContainsKey(account.LoginName))
                    {
                        _devAccountByLoginName.Add(account.LoginName, accountState);
                    }
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        private class MessageHandle :
            IHandler<AddDeveloperCommand>,
            IHandler<DeveloperUpdatedEvent>,
            IHandler<DeveloperRemovedEvent>,
            IHandler<RemoveDeveloperCommand>,
            IHandler<DeveloperAddedEvent>
        {
            private readonly SysUserSet _set;

            internal MessageHandle(SysUserSet set)
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
                messageDispatcher.Register((IHandler<AddDeveloperCommand>)this);
                messageDispatcher.Register((IHandler<DeveloperAddedEvent>)this);
                messageDispatcher.Register((IHandler<DeveloperUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveDeveloperCommand>)this);
                messageDispatcher.Register((IHandler<DeveloperRemovedEvent>)this);
            }

            public void Handle(AddDeveloperCommand message)
            {
                this.Handle(message.AcSession, message.AccountId, true);
            }

            public void Handle(DeveloperAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateDeveloperAddedEvent))
                {
                    return;
                }

                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid accountId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var devAccountById = _set._devAccountById;
                var devAccountByLoginName = _set._devAccountByLoginName;
                var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
                var developerRepository = acDomain.RetrieveRequiredService<IRepository<DeveloperId>>();
                DeveloperId entity;
                lock (Locker)
                {
                    var account = accountRepository.GetByKey(accountId);
                    if (account == null)
                    {
                        throw new ValidationException("账户不存在");
                    }
                    if (devAccountById.ContainsKey(accountId))
                    {
                        throw new ValidationException("给定标识标识的开发人员已经存在" + accountId);
                    }
                    entity = new DeveloperId
                    {
                        Id = accountId
                    };
                    try
                    {
                        var accountState = AccountState.Create(account);
                        devAccountById.Add(accountId, accountState);
                        devAccountByLoginName.Add(account.LoginName, accountState);
                        if (isCommand)
                        {
                            developerRepository.Add(entity);
                            developerRepository.Context.Commit();
                        }
                    }
                    catch
                    {
                        devAccountById.Remove(accountId);
                        devAccountByLoginName.Remove(account.LoginName);
                        developerRepository.Context.Rollback();
                        throw;
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateDeveloperAddedEvent(acSession, entity));
                }
            }

            private class PrivateDeveloperAddedEvent : DeveloperAddedEvent, IPrivateEvent
            {
                internal PrivateDeveloperAddedEvent(IAcSession acSession, DeveloperId source) : base(acSession, source) { }
            }

            public void Handle(DeveloperUpdatedEvent message)
            {
                var devAccountById = _set._devAccountById;
                var devAccountByLoginName = _set._devAccountByLoginName;
                var entity = message.Source as AccountBase;
                AccountState oldState;
                if (!devAccountById.TryGetValue(message.Source.Id, out oldState))
                {
                    throw new AnycmdException("给定标识的用户不存在");
                }
                var newState = AccountState.Create(entity);
                devAccountById[message.Source.Id] = newState;
                if (!devAccountByLoginName.ContainsKey(newState.LoginName))
                {
                    devAccountByLoginName.Add(newState.LoginName, newState);
                    devAccountByLoginName.Remove(oldState.LoginName);
                }
                else
                {
                    devAccountByLoginName[newState.LoginName] = newState;
                }
            }

            public void Handle(RemoveDeveloperCommand message)
            {
                this.HandleRemove(message.AcSession, message.AccountId, true);
            }

            public void Handle(DeveloperRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateDeveloperRemovedEvent))
                {
                    return;
                }
                this.HandleRemove(message.AcSession, message.Source.Id, false);
            }

            private void HandleRemove(IAcSession acSession, Guid accountId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var devAccountById = _set._devAccountById;
                var devAccountByLoginName = _set._devAccountByLoginName;
                var developerRepository = acDomain.RetrieveRequiredService<IRepository<DeveloperId>>();
                if (!devAccountById.ContainsKey(accountId))
                {
                    return;
                }
                var bkState = devAccountById[accountId];
                DeveloperId entity;
                lock (Locker)
                {
                    if (!devAccountById.ContainsKey(accountId))
                    {
                        return;
                    }
                    entity = developerRepository.GetByKey(accountId);
                    if (entity == null)
                    {
                        return;
                    }
                    try
                    {
                        devAccountById.Remove(accountId);
                        devAccountByLoginName.Remove(bkState.LoginName);
                        if (isCommand)
                        {
                            developerRepository.Remove(entity);
                            developerRepository.Context.Commit();
                        }
                    }
                    catch
                    {
                        devAccountById.Add(accountId, bkState);
                        devAccountByLoginName.Add(bkState.LoginName, bkState);
                        developerRepository.Context.Rollback();
                        throw;
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateDeveloperRemovedEvent(acSession, entity));
                }
            }

            private class PrivateDeveloperRemovedEvent : DeveloperRemovedEvent, IPrivateEvent
            {
                internal PrivateDeveloperRemovedEvent(IAcSession acSession, DeveloperId source) : base(acSession, source) { }
            }
        }
    }
}
