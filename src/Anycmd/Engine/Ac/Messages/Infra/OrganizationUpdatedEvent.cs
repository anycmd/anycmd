
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class OrganizationUpdatedEvent : DomainEvent
    {
        public OrganizationUpdatedEvent(OrganizationBase source, IOrganizationUpdateIo input)
            : base(source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IOrganizationUpdateIo Input { get; private set; }
    }
}
