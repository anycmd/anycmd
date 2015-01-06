using System;

namespace Anycmd.Engine.Edi.InOuts
{

    public interface IArchiveCreateIo : IEntityCreateInput
    {
        string Description { get; }
        Guid OntologyId { get; }
        string RdbmsType { get; }
        string Title { get; }
    }
}
