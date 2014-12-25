
namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;
    using System;

    public interface INodeOntologyOrganizationCreateIo : IEntityCreateInput
    {
        Guid NodeId { get; }
        Guid OntologyId { get; }
        Guid OrganizationId { get; }
    }
}
