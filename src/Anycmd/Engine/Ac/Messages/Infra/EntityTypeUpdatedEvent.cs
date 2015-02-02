
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeUpdatedEvent : DomainEvent
    {
        public EntityTypeUpdatedEvent(IAcSession userSession, EntityTypeBase source, IEntityTypeUpdateIo input)
            : base(userSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IEntityTypeUpdateIo Input { get; private set; }
    }
}
