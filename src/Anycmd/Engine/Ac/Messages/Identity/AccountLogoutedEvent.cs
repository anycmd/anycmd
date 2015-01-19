
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLogoutedEvent : DomainEvent
    {
        public AccountLogoutedEvent(IUserSession userSession, AccountBase source)
            : base(userSession, source)
        {
        }
    }
}