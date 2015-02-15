
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface ITopicUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        string Name { get; }
    }
}
