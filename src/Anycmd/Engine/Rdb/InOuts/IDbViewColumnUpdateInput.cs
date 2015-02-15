
namespace Anycmd.Engine.Rdb.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IDbViewColumnUpdateInput : IAnycmdInput
    {
        string Id { get; }
        Guid DatabaseId { get; }
        string Description { get; }
    }
}
