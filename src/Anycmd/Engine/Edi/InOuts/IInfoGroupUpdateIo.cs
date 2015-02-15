
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IInfoGroupUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
