
namespace Anycmd.Engine.Edi.InOuts
{

    public interface IArchiveUpdateIo : IEntityUpdateInput
    {
        string Description { get; }
        string Title { get; }
    }
}
