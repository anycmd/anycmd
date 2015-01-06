using System;

namespace Anycmd.Engine.Edi.InOuts
{

    public interface IInfoDicItemCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        Guid InfoDicId { get; }
        int IsEnabled { get; }
        string Level { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
