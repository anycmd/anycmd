
namespace Anycmd.Engine.Ac.Abstractions.Rbac
{
    using Model;
    using System;

    /// <summary>
    /// 用户会话基类。
    /// </summary>
    public abstract class UserSessionBase : EntityBase, IUserSessionEntity
    {
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string LoginName { get; set; }
        public Guid AccountId { get; set; }
        public int IsEnabled { get; set; }
        public string Description { get; set; }
    }
}
