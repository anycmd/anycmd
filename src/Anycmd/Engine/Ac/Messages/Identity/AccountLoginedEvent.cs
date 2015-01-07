
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AccountLoginedEvent : DomainEvent
    {
        public AccountLoginedEvent(AccountBase source) : base(source) { }
    }
}