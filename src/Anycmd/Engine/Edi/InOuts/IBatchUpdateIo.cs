
namespace Anycmd.Engine.Edi.InOuts
{

    public interface IBatchUpdateIo : IEntityUpdateInput
    {
        string Description { get; set; }
        string Title { get; set; }
    }
}
