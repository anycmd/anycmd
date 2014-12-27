
namespace Anycmd.Engine.Edi.InOuts
{
    using Model;

    public interface IBatchUpdateIo : IEntityUpdateInput
    {
        string Description { get; set; }
        string Title { get; set; }
    }
}
