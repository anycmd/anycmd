using System;

namespace Anycmd.Engine.Edi.InOuts
{

    public interface ITopicCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        string Name { get; }
        bool IsAllowed { get; }
        Guid OntologyId { get; }
    }
}
