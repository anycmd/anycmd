
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    public class GroupUpdatedEvent : DomainEvent
    {
        public GroupUpdatedEvent(GroupBase source, IGroupUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IGroupUpdateIo Output { get; private set; }
    }
}