
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Host;
    using Host.Ac.Identity;
    using Host.Impl;
    using Model;
    using Repositories;
    using System;
    using System.Security.Principal;

    /// <summary>
    /// 表示用户会话业务实体。
    /// </summary>
    public class UserSessionState : IUserSession, IStateObject
    {
        public static readonly UserSessionState Empty = new UserSessionState
        {
            _acDomain = EmptyAcDomain.SingleInstance,
            _principal = new AnycmdPrincipal(EmptyAcDomain.SingleInstance, new UnauthenticatedIdentity()),
            _account = AccountState.Empty,
            _id = Guid.Empty
        };

        private IAcDomain _acDomain;
        private Guid _id;
        private IPrincipal _principal;
        private readonly Guid _accountId;
        private AccountState _account;
        private AccountPrivilege _accountPrivilege;

        private UserSessionState()
        {

        }

        internal UserSessionState(IAcDomain host, Guid sessionId, IPrincipal principal, AccountState account)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            _acDomain = host;
            _id = sessionId;
            _principal = principal;
            _account = account;
            _accountId = account.Id;
        }

        internal UserSessionState(IAcDomain host, UserSessionBase userSessionEntity)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (userSessionEntity == null)
            {
                throw new ArgumentNullException("userSessionEntity");
            }
            var principal = new AnycmdPrincipal(host, new AnycmdIdentity(userSessionEntity.AuthenticationType, userSessionEntity.IsAuthenticated, userSessionEntity.LoginName));
            _acDomain = host;
            _id = userSessionEntity.Id;
            _principal = principal;
            _accountId = userSessionEntity.AccountId;
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public IPrincipal Principal
        {
            get { return _principal; }
        }

        public AccountPrivilege AccountPrivilege
        {
            get
            {
                if (_accountPrivilege == null)
                {
                    _accountPrivilege = new AccountPrivilege(this.AcDomain, this._accountId);
                    string msg;
                    if (!AcDomain.DsdSetSet.CheckRoles(_accountPrivilege.AuthorizedRoles, out msg))
                    {
                        throw new Exceptions.ValidationException(msg);
                    }
                }
                return _accountPrivilege;
            }
        }

        /// <summary>
        /// 工人
        /// </summary>
        public AccountState Account
        {
            get
            {
                if (_account == null)
                {
                    var accountRepository = AcDomain.RetrieveRequiredService<IRepository<Account>>();
                    _account = AccountState.Create(accountRepository.GetByKey(this._accountId));
                }
                return _account;
            }
        }
    }
}
