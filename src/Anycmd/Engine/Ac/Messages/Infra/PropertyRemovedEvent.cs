
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyRemovedEvent : DomainEvent
    {
        public PropertyRemovedEvent(PropertyBase source)
            : base(source)
        {
        }
    }
}
