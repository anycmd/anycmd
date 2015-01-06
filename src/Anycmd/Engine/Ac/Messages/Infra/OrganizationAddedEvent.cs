
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class OrganizationAddedEvent : EntityAddedEvent<IOrganizationCreateIo>
    {
        public OrganizationAddedEvent(OrganizationBase source, IOrganizationCreateIo input)
            : base(source, input)
        {
        }
    }
}
