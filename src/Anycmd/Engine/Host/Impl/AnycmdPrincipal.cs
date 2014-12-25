
namespace Anycmd.Engine.Host.Impl
{
    using Exceptions;
    using System;
    using System.Security.Principal;

    public sealed class AnycmdPrincipal : IPrincipal
    {
        private readonly IAcDomain _host;

        public AnycmdPrincipal(IAcDomain host, IIdentity identity)
        {
            this._host = host;
            this.Identity = identity;
        }

        public AnycmdPrincipal(IAcDomain host, IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            this._host = host;
            this.Identity = principal.Identity;
        }

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

            return _host.UserSession.AccountPrivilege.AuthorizedRoleIds.Contains(roleId);
        }
    }
}
