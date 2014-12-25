using System;

namespace Anycmd.Engine.Host.Edi.InOuts
{
    using Model;

    public interface INodeElementCareCreateIo : IEntityCreateInput
    {
        Guid ElementId { get; }
        Guid NodeId { get; }
        bool IsInfoIdItem { get; }
    }
}
