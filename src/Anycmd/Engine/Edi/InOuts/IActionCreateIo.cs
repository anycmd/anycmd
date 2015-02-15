
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IActionCreateIo : IEntityCreateInput
    {
        string Description { get; }
        string IsAllowed { get; }
        string IsAudit { get; }
        bool IsPersist { get; }
        string Name { get; }
        Guid OntologyId { get; }
        int SortCode { get; }
        string Verb { get; }
    }
}
