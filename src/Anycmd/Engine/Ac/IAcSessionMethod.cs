
namespace Anycmd.Engine.Ac
{
    using Accounts;
    using System;
    using System.Collections.Generic;

    public interface IAcSessionMethod
    {
        Guid Id { get; }

        /// <summary>
        /// 签入用户
        /// </summary>
        Action<IAcDomain, Dictionary<string, object>> SignIn { get; }

        /// <summary>
        /// 签出用户
        /// </summary>
        Action<IAcDomain, IAcSession> SignOut { get; }

        /// <summary>
        /// 用户签出后执行的过程
        /// </summary>
        Action<IAcDomain, Guid> SignOuted { get; }

        /// <summary>
        /// 从持久层加载给定标识的Account记录。
        /// </summary>
        Func<IAcDomain, Guid, IAccount> GetAccountById { get; }

        /// <summary>
        /// 从持久层加载给定的登录名标识的Account记录。
        /// </summary>
        Func<IAcDomain, string, IAccount> GetAccountByLoginName { get; }

        /// <summary>
        /// 从持久层加载给定标识的AcSessionEntity记录。
        /// </summary>
        Func<IAcDomain, Guid, IAcSessionEntity> GetAcSessionEntity { get; }

        /// <summary>
        /// 读取给定的登录名标识的AcSession对象。
        /// </summary>
        Func<IAcDomain, string, IAcSession> GetAcSession { get; }

        /// <summary>
        /// 添加给定的AcSessionEntity记录到持久层。
        /// </summary>
        Action<IAcDomain, IAcSessionEntity> AddAcSession { get; }

        /// <summary>
        /// 更新持久层中的给定的AcSessionEntity记录。
        /// </summary>
        Action<IAcDomain, IAcSessionEntity> UpdateAcSession { get; }

        /// <summary>
        /// 从持久层删除给定标识的AcSessionEntity记录。
        /// <remarks>
        /// 会话不应该经常删除，会话级的权限依赖于会话的持久跟踪。用户退出系统只需要清空该用户的
        /// 内存会话记录和更新数据库中的会话记录为IsAuthenticated为false而不需要删除持久的AcSession。
        /// </remarks>
        /// </summary>
        Action<IAcDomain, Guid> DeleteAcSession { get; }
    }
}
