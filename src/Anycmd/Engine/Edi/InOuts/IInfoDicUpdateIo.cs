
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IInfoDicUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
