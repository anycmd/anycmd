
namespace Anycmd.Engine.Edi.InOuts
{
    using Model;

    public interface IArchiveUpdateIo : IEntityUpdateInput
    {
        string Description { get; }
        string Title { get; }
    }
}
