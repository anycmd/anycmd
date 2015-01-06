
namespace Anycmd.Engine.Edi.InOuts
{
    using System;

    public interface INodeOntologyOrganizationCreateIo : IEntityCreateInput
    {
        Guid NodeId { get; }
        Guid OntologyId { get; }
        Guid OrganizationId { get; }
    }
}
