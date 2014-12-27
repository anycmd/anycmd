
namespace Anycmd.Engine.Ac.Messages.Identity
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