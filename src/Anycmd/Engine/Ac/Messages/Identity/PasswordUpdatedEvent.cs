
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class PasswordUpdatedEvent : DomainEvent
    {
        public PasswordUpdatedEvent(IAcSession acSession, AccountBase source)
            : base(acSession, source)
        {
            this.Password = source.Password;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; private set; }
    }
}
