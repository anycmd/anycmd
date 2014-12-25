
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionRemovedEvent : DomainEvent
    {
        public FunctionRemovedEvent(FunctionBase source)
            : base(source)
        {
        }
    }
}
