using System;

namespace Anycmd.Engine.Edi.InOuts
{

    public interface INodeOntologyCareCreateIo : IEntityCreateInput
    {
        Guid NodeId { get; }
        Guid OntologyId { get; }
    }
}
