
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountLoginedEvent : DomainEvent
    {
        public AccountLoginedEvent(IAcSession acSession, IAccount source) : base(acSession, source) { }
    }
}