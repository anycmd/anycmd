
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
        internal bool IsPrivate { get; set; }
    }
}
