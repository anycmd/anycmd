
namespace Anycmd.Engine.Rdb.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IDbTableUpdateInput : IAnycmdInput
    {
        string Id { get; }
        Guid DatabaseId { get; }
        string Description { get; }
    }
}
