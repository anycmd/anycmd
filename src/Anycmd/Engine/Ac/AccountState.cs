
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Abstractions.Identity;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 表示账户业务实体类型。
    /// </summary>
    public sealed class AccountState : StateObject<AccountState>, IAccount, IAcElement
    {
        private string _name;
        private int _numberId;
        private DateTime? _createOn;
        private string _loginName;
        private string _code;
        private string _email;
        private string _mobile;
        private string _qq;
        private string _nickname;
        private string _blogUrl;
        private string _password;
        private string _auditState;
        private DateTime? _allowStartTime;
        private DateTime? _allowEndTime;
        private DateTime? _lockStartTime;
        private DateTime? _lockEndTime;
        private DateTime? _firtLoginOn;
        private DateTime? _priviousLoginOn;
        private int? _loginCount;
        private string _ipAddress;
        private int _isEnabled;

        /// <summary>
        /// 空账户
        /// </summary>
        public static readonly AccountState Empty = new AccountState(Guid.Empty)
        {
            _numberId = int.MinValue,
            _createOn = SystemTime.MinDate,
            _loginName = string.Empty,
            _code = string.Empty,
            _email = string.Empty,
            _mobile = string.Empty,
            _name = string.Empty,
            _nickname = string.Empty,
            _qq = string.Empty,
            _blogUrl = string.Empty,
            _password = string.Empty,
            _auditState = string.Empty,
            _allowStartTime = null,
            _allowEndTime = null,
            _lockStartTime = null,
            _loginCount = 0,
            _lockEndTime = null,
            _priviousLoginOn = null,
            _firtLoginOn = null,
            _ipAddress = string.Empty,
            _isEnabled = 0
        };

        private AccountState(Guid id) : base(id) { }

        public static AccountState Create(IAccount account)
        {
            if (account == null)
            {
                return Empty;
            }
            return new AccountState(account.Id)
            {
                _numberId = account.NumberId,
                _loginName = account.LoginName,
                _createOn = account.CreateOn,
                _code = account.Code,
                _email = account.Email,
                _mobile = account.Mobile,
                _name = account.Name,
                _nickname = account.Nickname,
                _qq = account.Qq,
                _blogUrl = account.BlogUrl,
                _password = account.Password,
                _auditState = account.AuditState,
                _allowStartTime = account.AllowStartTime,
                _allowEndTime = account.AllowEndTime,
                _lockStartTime = account.LockStartTime,
                _loginCount = account.LoginCount,
                _lockEndTime = account.LockEndTime,
                _priviousLoginOn = account.PreviousLoginOn,
                _firtLoginOn = account.FirstLoginOn,
                _ipAddress = account.IpAddress,
                _isEnabled = account.IsEnabled
            };
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.Account; }
        }

        public int NumberId
        {
            get { return _numberId; }
        }

        public string LoginName
        {
            get { return _loginName; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Nickname
        {
            get { return _nickname; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string Email
        {
            get { return _email; }
        }

        public string Qq
        {
            get { return _qq; }
        }

        public string Mobile
        {
            get { return _mobile; }
        }

        public string BlogUrl
        {
            get { return _blogUrl; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public string Password
        {
            get { return _password; }
        }

        public string AuditState
        {
            get { return _auditState; }
        }

        public DateTime? AllowStartTime
        {
            get { return _allowStartTime; }
        }

        public DateTime? AllowEndTime
        {
            get { return _allowEndTime; }
        }

        public DateTime? LockStartTime
        {
            get { return _lockStartTime; }
        }

        public DateTime? LockEndTime
        {
            get { return _lockEndTime; }
        }

        public DateTime? FirstLoginOn
        {
            get { return _firtLoginOn; }
            set
            {
                // 什么都不做
            }
        }

        public DateTime? PreviousLoginOn
        {
            get { return _priviousLoginOn; }
            set
            {
                // 什么都不做
            }
        }

        public int? LoginCount
        {
            get { return _loginCount; }
            set
            {
                // 什么都不做
            }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                // 什么都不做
            }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    NumberId:'{1}',
    LoginName:'{2}',
    Name:'{3}',
    Nickname:'{4}',
    Code:'{5}',
    Email:'{6}',
    Qq:'{7}',
    Mobile:'{8}',
    BlogUrl:'{9}',
    CreateOn:'{10}'
}}", Id, NumberId, LoginName, Name, Nickname, Code, Email, Qq, Mobile, BlogUrl, CreateOn);
        }

        protected override bool DoEquals(AccountState other)
        {
            return Id == other.Id &&
                LoginName == other.LoginName &&
                NumberId == other.NumberId &&
                Password == other.Password &&
                Name == other.Name &&
                Nickname == other.Nickname &&
                Code == other.Code &&
                AuditState == other.AuditState &&
                AllowStartTime == other.AllowStartTime &&
                AllowEndTime == other.AllowEndTime &&
                LockStartTime == other.LockStartTime &&
                LockEndTime == other.LockEndTime &&
                FirstLoginOn == other.FirstLoginOn &&
                PreviousLoginOn == other.PreviousLoginOn &&
                LoginCount == other.LoginCount &&
                IpAddress == other.IpAddress &&
                IsEnabled == other.IsEnabled &&
                Email == other.Email &&
                Qq == other.Qq &&
                Mobile == other.Mobile &&
                BlogUrl == other.BlogUrl;
        }
    }
}
