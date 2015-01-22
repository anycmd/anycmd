
namespace Anycmd.Engine.Edi.InOuts
{
    public interface IStateCodeUpdateInput : IEntityUpdateInput
    {
        string ReasonPhrase { get; }
        string Description { get; }
    }
}
