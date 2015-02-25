
namespace Anycmd.Engine.Ac.Functions
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建系统功能时的输入或输出参数类型。
    /// </summary>
    public interface IFunctionCreateIo : IEntityCreateInput
    {
        string Code { get; }
        bool IsManaged { get; }
        int IsEnabled { get; }
        string Description { get; }
        Guid DeveloperId { get; }
        Guid ResourceTypeId { get; }
        int SortCode { get; }
    }
}
