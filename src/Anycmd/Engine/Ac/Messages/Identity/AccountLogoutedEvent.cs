
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLogoutedEvent : DomainEvent
    {
        public AccountLogoutedEvent(AccountBase source, IUserSession userSession) : base(source)
        {
            if (userSession == null)
            {
                throw new ArgumentNullException("userSession");
            }
            this.UserSession = userSession;
        }

        public IUserSession UserSession { get; private set; }
    }
}