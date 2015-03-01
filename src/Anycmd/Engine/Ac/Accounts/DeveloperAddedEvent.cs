
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DeveloperAddedEvent : DomainEvent
    {
        public DeveloperAddedEvent(IAcSession acSession, DeveloperId source) : base(acSession, source) { }

        internal DeveloperAddedEvent(IAcSession acSession, DeveloperId source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
