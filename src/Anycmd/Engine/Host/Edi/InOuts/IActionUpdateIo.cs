
namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;

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
