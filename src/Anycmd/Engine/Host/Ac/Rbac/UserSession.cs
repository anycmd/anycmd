
namespace Anycmd.Engine.Host.Ac.Rbac
{
    using Engine.Ac.Abstractions.Rbac;
    using Model;

    /// <summary>
    /// 表示用户会话数据访问实体。
    /// </summary>
    public class UserSession : UserSessionBase, IAggregateRoot
    {
        public UserSession() { }
    }
}
