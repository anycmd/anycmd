using System;

namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;

    public interface IBatchCreateIo : IEntityCreateInput
    {
        string Description { get; }
        bool? IncludeDescendants { get; }
        Guid NodeId { get; }
        Guid OntologyId { get; }
        string OrganizationCode { get; }
        string Title { get; }
        string Type { get; }
    }
}
