
namespace Anycmd.Ac.ViewModels.Infra.AppSystemViewModels
{
    using Engine.Ac;
    using Engine.Ac.Abstractions.Identity;
    using Exceptions;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemTr
    {
        private AccountState _principal;
        private readonly IAcDomain _host;

        private AppSystemTr(IAcDomain host)
        {
            this._host = host;
        }

        public static AppSystemTr Create(IAcDomain host, AppSystemState appSystem)
        {
            return new AppSystemTr(host)
            {
                Code = appSystem.Code,
                CreateOn = appSystem.CreateOn,
                Icon = appSystem.Icon,
                Id = appSystem.Id,
                IsEnabled = appSystem.IsEnabled,
                Name = appSystem.Name,
                PrincipalId = appSystem.PrincipalId,
                SortCode = appSystem.SortCode,
                SsoAuthAddress = appSystem.SsoAuthAddress
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 单点登录Http认证接口地址
        /// </summary>
        public string SsoAuthAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public Guid? PrincipalId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrincipalName
        {
            get
            {
                if (this.PrincipalId.HasValue)
                {
                    return this.Principal.LoginName;
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 字符串类型，枚举值：“禁用”或“正常”
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }

        private IAccount Principal
        {
            get
            {
                if (this.PrincipalId.HasValue)
                {
                    if (!_host.SysUsers.TryGetDevAccount(this.PrincipalId.Value, out _principal))
                    {
                        throw new ValidationException("意外的开发人员标识" + this.PrincipalId);
                    }
                    return _principal;
                }
                return null;
            }
        }
    }
}
