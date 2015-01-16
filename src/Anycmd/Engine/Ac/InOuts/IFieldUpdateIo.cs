
namespace Anycmd.Engine.Ac.InOuts
{
    public interface IFieldUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Name { get; }
        string Description { get; }
        string Icon { get; }
        int SortCode { get; }
    }
}
