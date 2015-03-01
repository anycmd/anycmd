
namespace Anycmd.Engine.Ac.AppSystems
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppSystemRemovedEvent : DomainEvent
    {
        public AppSystemRemovedEvent(IAcSession acSession, AppSystemBase source)
            : base(acSession, source)
        {
        }

        internal AppSystemRemovedEvent(IAcSession acSession, AppSystemBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
