using System;

namespace Anycmd.Engine.Edi.InOuts
{

    public interface IInfoGroupCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        string Name { get; }
        Guid OntologyId { get; }
        int SortCode { get; }
    }
}
