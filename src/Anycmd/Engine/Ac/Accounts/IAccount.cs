
namespace Anycmd.Engine.Ac.Accounts
{
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是账户
    /// </summary>
    public interface IAccount : IEntity
    {
        /// <summary>
        /// 数字标识
        /// <remarks>
        /// 数字标识作为对人类友好的标识提供给外部。如审计系统。审批工作流中的角色采用数字标识。
        /// </remarks>
        /// </summary>
        int NumberId { get; }

        /// <summary>
        /// 登录名
        /// </summary>
        string LoginName { get; }

        string Password { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 网名
        /// </summary>
        string Nickname { get; }

        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }

        string Email { get; }

        string Qq { get; }

        string Mobile { get; }

        string BlogUrl { get; }

        string AuditState { get; }

        DateTime? AllowStartTime { get; }

        DateTime? AllowEndTime { get; }

        DateTime? LockStartTime { get; }

        DateTime? LockEndTime { get; }

        DateTime? FirstLoginOn { get; set; }

        DateTime? PreviousLoginOn { get; set; }

        int? LoginCount { get; set; }

        string IpAddress { get; set; }

        DateTime? CreateOn { get; }

        int IsEnabled { get; }
    }
}
