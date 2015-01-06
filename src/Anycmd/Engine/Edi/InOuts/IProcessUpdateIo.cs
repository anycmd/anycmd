
namespace Anycmd.Engine.Edi.InOuts
{

    public interface IProcessUpdateIo : IEntityUpdateInput
    {
        string Name { get; }

        int IsEnabled { get; }
    }
}
