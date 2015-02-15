
namespace Anycmd.Engine.Ac.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IFieldCreateIo : IEntityCreateInput
    {
        Guid ResourceTypeId { get; }
        string Code { get; }
        string Name { get; }
        string Description { get; }
        string Icon { get; }
        int SortCode { get; }
    }
}
