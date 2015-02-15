
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IBatchUpdateIo : IEntityUpdateInput
    {
        string Description { get; set; }
        string Title { get; set; }
    }
}
