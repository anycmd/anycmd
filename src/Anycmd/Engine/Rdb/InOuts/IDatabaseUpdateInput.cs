
namespace Anycmd.Engine.Rdb.InOuts
{
    using Engine;
    using System;

    public interface IDatabaseUpdateInput : IEntityUpdateInput
    {
        Guid Id { get; }

        string DataSource { get; }

        string Description { get; }
    }
}
