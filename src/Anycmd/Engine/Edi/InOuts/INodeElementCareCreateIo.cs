
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface INodeElementCareCreateIo : IEntityCreateInput
    {
        Guid ElementId { get; }
        Guid NodeId { get; }
        bool IsInfoIdItem { get; }
    }
}
