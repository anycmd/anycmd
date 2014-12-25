
namespace Anycmd.Engine.Host
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 表示该接口的实现类是用户会话服务。
    /// </summary>
    public interface IUserSessionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="sessionId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        IUserSession CreateSession(IAcDomain host, Guid sessionId, AccountState account);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="sessionId"></param>
        void DeleteSession(IAcDomain host, Guid sessionId);
    }
}
