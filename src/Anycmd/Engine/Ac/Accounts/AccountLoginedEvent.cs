
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLoginedEvent : DomainEvent
    {
        public AccountLoginedEvent(IAcSession acSession, IAccount source) : base(acSession, source) { }
    }
}