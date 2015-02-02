
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLogoutedEvent : DomainEvent
    {
        public AccountLogoutedEvent(IAcSession acSession, AccountBase source)
            : base(acSession, source)
        {
        }
    }
}