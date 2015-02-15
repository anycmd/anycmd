
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IProcessUpdateIo : IEntityUpdateInput
    {
        string Name { get; }

        int IsEnabled { get; }
    }
}
