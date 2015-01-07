
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class PasswordUpdatedEvent : DomainEvent
    {
        public PasswordUpdatedEvent(AccountBase source)
            : base(source)
        {
            this.Password = source.Password;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; private set; }
    }
}
