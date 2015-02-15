
namespace Anycmd.Engine.Rdb.InOuts
{
    using Engine.InOuts;

    public interface IDatabaseUpdateInput : IEntityUpdateInput
    {
        string DataSource { get; }

        string Description { get; }
    }
}
