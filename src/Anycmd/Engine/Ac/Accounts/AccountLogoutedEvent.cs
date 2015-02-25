
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLogoutedEvent : DomainEvent
    {
        public AccountLogoutedEvent(IAcSession acSession, IAccount source)
            : base(acSession, source)
        {
        }
    }
}