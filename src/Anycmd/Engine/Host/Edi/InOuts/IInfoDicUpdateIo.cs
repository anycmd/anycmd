
namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;

    public interface IInfoDicUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
