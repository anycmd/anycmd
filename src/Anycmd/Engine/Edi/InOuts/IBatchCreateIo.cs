
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IBatchCreateIo : IEntityCreateInput
    {
        string Description { get; }
        bool? IncludeDescendants { get; }
        Guid NodeId { get; }
        Guid OntologyId { get; }
        string CatalogCode { get; }
        string Title { get; }
        string Type { get; }
    }
}
