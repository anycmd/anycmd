
namespace Anycmd.Engine.Edi.InOuts
{
    using Model;

    public interface IInfoDicCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
