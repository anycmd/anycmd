
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PasswordUpdatedEvent : DomainEvent
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
