
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLogoutedEvent : DomainEvent
    {
        public AccountLogoutedEvent(AccountBase source) : base(source) { }
    }
}