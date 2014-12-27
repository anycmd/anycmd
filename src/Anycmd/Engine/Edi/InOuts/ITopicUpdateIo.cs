
namespace Anycmd.Engine.Edi.InOuts
{
    using Model;

    public interface ITopicUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        string Name { get; }
    }
}
