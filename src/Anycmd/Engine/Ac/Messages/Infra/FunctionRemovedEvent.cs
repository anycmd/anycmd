
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionRemovedEvent : DomainEvent
    {
        public FunctionRemovedEvent(IAcSession userSession, FunctionBase source)
            : base(userSession, source)
        {
        }
    }
}
