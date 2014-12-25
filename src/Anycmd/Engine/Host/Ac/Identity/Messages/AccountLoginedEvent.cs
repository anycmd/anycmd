
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLoginedEvent : DomainEvent
    {
        public AccountLoginedEvent(AccountBase source) : base(source) { }
    }
}