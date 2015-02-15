
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IArchiveUpdateIo : IEntityUpdateInput
    {
        string Description { get; }
        string Title { get; }
    }
}
