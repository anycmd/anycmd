using System;

namespace Anycmd.Engine.Edi.InOuts
{
    using Model;

    public interface IArchiveCreateIo : IEntityCreateInput
    {
        string Description { get; }
        Guid OntologyId { get; }
        string RdbmsType { get; }
        string Title { get; }
    }
}
