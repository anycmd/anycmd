
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
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
