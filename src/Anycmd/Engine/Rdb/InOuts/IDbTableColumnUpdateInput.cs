
namespace Anycmd.Engine.Rdb.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IDbTableColumnUpdateInput : IAnycmdInput
    {
        string Id { get; }
        Guid DatabaseId { get; }
        string Description { get; }
    }
}
