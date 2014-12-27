
namespace Anycmd.Engine.Ac.Messages.Infra
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
