
namespace Anycmd.Engine.Host.Ac.MemorySets.Impl
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions.Identity;
    using Exceptions;
    using Extensions;
    using Host;
    using Identity;
    using Identity.Messages;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using loginName = System.String;

    public sealed class SysUserSet : ISysUserSet
    {
        public static readonly ISysUserSet Empty = new SysUserSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, AccountState> _devAccountById = new Dictionary<Guid, AccountState>();
        private readonly Dictionary<loginName, AccountState> _devAccountByLoginName = new Dictionary<loginName, AccountState>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        public SysUserSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
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
            if (developerCode == null)
            {
                developer = null;
                return false;
            }

            return _devAccountByLoginName.TryGetValue(developerCode, out developer);
        }

        public bool TryGetDevAccount(Guid accountId, out AccountState developer)
        {
            if (!_initialized)
            {
                Init();
            }

            return _devAccountById.TryGetValue(accountId, out developer);
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
            if (!_initialized)
            {
                lock (this)
                {
                    if (!_initialized)
                    {
                        _devAccountById.Clear();
                        _devAccountByLoginName.Clear();
                        var accounts = _host.GetRequiredService<IOriginalHostStateReader>().GetAllDevAccounts();
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
                    }
                }
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

            public MessageHandle(SysUserSet set)
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
                messageDispatcher.Register((IHandler<AddDeveloperCommand>)this);
                messageDispatcher.Register((IHandler<DeveloperAddedEvent>)this);
                messageDispatcher.Register((IHandler<DeveloperUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveDeveloperCommand>)this);
                messageDispatcher.Register((IHandler<DeveloperRemovedEvent>)this);
            }

            public void Handle(AddDeveloperCommand message)
            {
                this.Handle(message.AccountId, true);
            }

            public void Handle(DeveloperAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateDeveloperAddedEvent))
                {
                    return;
                }

                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid accountId, bool isCommand)
            {
                var host = _set._host;
                var devAccountById = _set._devAccountById;
                var devAccountByLoginName = _set._devAccountByLoginName;
                var accountRepository = host.GetRequiredService<IRepository<Account>>();
                var developerRepository = host.GetRequiredService<IRepository<DeveloperId>>();
                DeveloperId entity;
                lock (this)
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
                    host.MessageDispatcher.DispatchMessage(new PrivateDeveloperAddedEvent(entity));
                }
            }

            private class PrivateDeveloperAddedEvent : DeveloperAddedEvent
            {
                public PrivateDeveloperAddedEvent(DeveloperId source) : base(source) { }
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
                this.HandleRemove(message.AccountId, true);
            }

            public void Handle(DeveloperRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateDeveloperRemovedEvent))
                {
                    return;
                }
                this.HandleRemove(message.Source.Id, false);
            }

            private void HandleRemove(Guid accountId, bool isCommand)
            {
                var host = _set._host;
                var devAccountById = _set._devAccountById;
                var devAccountByLoginName = _set._devAccountByLoginName;
                var developerRepository = host.GetRequiredService<IRepository<DeveloperId>>();
                if (!devAccountById.ContainsKey(accountId))
                {
                    return;
                }
                var bkState = devAccountById[accountId];
                DeveloperId entity;
                lock (bkState)
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
                    host.MessageDispatcher.DispatchMessage(new PrivateDeveloperRemovedEvent(entity));
                }
            }

            private class PrivateDeveloperRemovedEvent : DeveloperRemovedEvent
            {
                public PrivateDeveloperRemovedEvent(DeveloperId source) : base(source) { }
            }
        }
    }
}
