
namespace Anycmd.Engine.Rdb.InOuts
{
    using Engine;

    public interface IDatabaseUpdateInput : IEntityUpdateInput
    {
        string DataSource { get; }

        string Description { get; }
    }
}
