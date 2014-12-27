
namespace Anycmd.Engine.Ac.Messages.Identity
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