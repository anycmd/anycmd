
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountLogoutedEvent : DomainEvent
    {
        public AccountLogoutedEvent(IAcSession acSession, IAccount source)
            : base(acSession, source)
        {
        }
    }
}