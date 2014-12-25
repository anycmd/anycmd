
namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;

    public interface IProcessUpdateIo : IEntityUpdateInput
    {
        string Name { get; }

        int IsEnabled { get; }
    }
}
