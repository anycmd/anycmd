
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemRemovedEvent : DomainEvent
    {
        public AppSystemRemovedEvent(AppSystemBase source)
            : base(source)
        {
        }
    }
}
