
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IActionUpdateIo : IEntityUpdateInput
    {
        string Description { get; }
        string IsAllowed { get; }
        string IsAudit { get; }
        bool IsPersist { get; }
        string Name { get; }
        int SortCode { get; }
        string Verb { get; }
    }
}
