using System;

namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;

    public interface INodeOntologyCareCreateIo : IEntityCreateInput
    {
        Guid NodeId { get; }
        Guid OntologyId { get; }
    }
}
