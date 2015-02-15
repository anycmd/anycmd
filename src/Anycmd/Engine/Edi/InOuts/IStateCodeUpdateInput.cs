
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IStateCodeUpdateInput : IEntityUpdateInput
    {
        string ReasonPhrase { get; }
        string Description { get; }
    }
}
