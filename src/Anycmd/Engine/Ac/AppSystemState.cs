
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Abstractions.Infra;
    using Exceptions;
    using System;
    using Util;

    /// <summary>
    /// 表示应用系统业务实体类型。
    /// </summary>
    public sealed class AppSystemState : StateObject<AppSystemState>, IAppSystem, IAcElement
    {
        public static readonly AppSystemState Empty = new AppSystemState(Guid.Empty)
        {
            _code = string.Empty,
            _createOn = SystemTime.MinDate,
            _icon = string.Empty,
            _isEnabled = 0,
            _name = string.Empty,
            _principalId = Guid.Empty,
            _sortCode = 0,
            _ssoAuthAddress = string.Empty
        };

        private string _code;
        private string _name;
        private int _sortCode;
        private Guid _principalId;
        private int _isEnabled;
        private string _ssoAuthAddress;
        private string _icon;
        private DateTime? _createOn;

        private AppSystemState(Guid id) : base(id) { }

        public static AppSystemState Create(IAcDomain host, AppSystemBase appSystem)
        {
            if (appSystem == null)
            {
                throw new ArgumentNullException("appSystem");
            }
            AccountState principal;
            if (!host.SysUserSet.TryGetDevAccount(appSystem.PrincipalId, out principal))
            {
                throw new AnycmdException("意外的应用系统负责人标识" + appSystem.PrincipalId);
            }
            return new AppSystemState(appSystem.Id)
            {
                _code = appSystem.Code,
                _name = appSystem.Name,
                _sortCode = appSystem.SortCode,
                _principalId = appSystem.PrincipalId,
                _isEnabled = appSystem.IsEnabled,
                _ssoAuthAddress = appSystem.SsoAuthAddress,
                _icon = appSystem.Icon,
                _createOn = appSystem.CreateOn,
            };
        }

        public AcElementType AcElementType
        {
            get { return AcElementType.AppSystem; }
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int SortCode
        {
            get
            {
                return _sortCode;
            }
        }

        public Guid PrincipalId
        {
            get
            {
                return _principalId;
            }
        }

        public int IsEnabled
        {
            get
            {
                return _isEnabled;
            }
        }

        public string SsoAuthAddress
        {
            get
            {
                return _ssoAuthAddress;
            }
        }

        public string Icon
        {
            get
            {
                return _icon;
            }
        }

        public DateTime? CreateOn
        {
            get
            {
                return _createOn;
            }
        }

        protected override bool DoEquals(AppSystemState other)
        {
            return
                Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                SortCode == other.SortCode &&
                IsEnabled == other.IsEnabled &&
                SsoAuthAddress == other.SsoAuthAddress &&
                Icon == other.Icon;
        }
    }
}
