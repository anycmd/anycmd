
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IArchiveCreateIo : IEntityCreateInput
    {
        string Description { get; }
        Guid OntologyId { get; }
        string RdbmsType { get; }
        string Title { get; }
    }
}
