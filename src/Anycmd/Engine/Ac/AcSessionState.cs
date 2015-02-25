
namespace Anycmd.Engine.Ac
{
    using Accounts;
    using Exceptions;
    using Host;
    using Host.Impl;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Principal;

    /// <summary>
    /// 表示用户会话业务实体。
    /// </summary>
    public class AcSessionState : IAcSession, IStateObject
    {
        public static readonly AcSessionState Empty;

        private readonly IAcDomain _acDomain;
        private readonly Guid _id;
        private readonly Guid _accountId;
        private AccountState _account;
        private AccountPrivilege _accountPrivilege;
        private static IAcSessionMethod _acMethod = null;

        public static IAcSessionMethod AcMethod
        {
            get { return _acMethod ?? (_acMethod = new DefaultAcSessionMethod()); }
            internal set { _acMethod = value; }
        }

        static AcSessionState()
        {
            Empty = new AcSessionState(EmptyAcDomain.SingleInstance, Guid.Empty, AccountState.Empty)
            {
                _accountPrivilege = null
            };
        }

        #region ctor
        private AcSessionState(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
        }

        internal AcSessionState(IAcDomain acDomain, Guid sessionId, AccountState account)
            : this(acDomain)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (account == AccountState.Empty)
            {
                Identity = new UnauthenticatedIdentity();
            }
            else
            {
                Identity = new AnycmdIdentity(account.LoginName);
            }
            _acDomain = acDomain;
            _id = sessionId;
            _account = account;
            _accountId = account.Id;
        }

        internal AcSessionState(IAcDomain acDomain, IAcSessionEntity acSessionEntity)
            : this(acDomain)
        {
            if (acSessionEntity == null)
            {
                throw new ArgumentNullException("acSessionEntity");
            }
            Identity = new AnycmdIdentity(acSessionEntity.LoginName);
            _acDomain = acDomain;
            _id = acSessionEntity.Id;
            _accountId = acSessionEntity.AccountId;
        }
        #endregion

        public Guid Id
        {
            get { return _id; }
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public AccountPrivilege AccountPrivilege
        {
            get
            {
                if (_accountPrivilege != null) return _accountPrivilege;
                _accountPrivilege = new AccountPrivilege(this.AcDomain, this);
                string msg;
                var authorizedRoleList = _accountPrivilege.AuthorizedRoles as IList<RoleState>;
                Debug.Assert(authorizedRoleList != null);
                if (!AcDomain.DsdSetSet.CheckRoles(authorizedRoleList, out msg))
                {
                    throw new ValidationException(msg);
                }
                return _accountPrivilege;
            }
        }

        /// <summary>
        /// 当事人账户
        /// </summary>
        public AccountState Account
        {
            get
            {
                if (_account != null) return _account;
                _account = AccountState.Create(AcMethod.GetAccountById(_acDomain, this._accountId));
                return _account;
            }
        }

        /// <summary>
        /// 当事人标识
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// .NET的IPrincipal接口的IsInRole方法基本是鸡肋。建议不要面向这个接口编程。
        /// </summary>
        /// <param name="role">单个角色标识。不支持复杂的带有分隔符的甚至带有逻辑运算的字符串。</param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            Guid roleId;
            if (!Guid.TryParse(role, out roleId))
            {
                throw new ValidationException("意外的角色标识" + role);
            }

            return this.AccountPrivilege.AuthorizedRoleIds.Contains(roleId);
        }
    }
}
