
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface ITopicCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        string Name { get; }
        bool IsAllowed { get; }
        Guid OntologyId { get; }
    }
}
