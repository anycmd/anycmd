
namespace Anycmd.Engine.Ac.InOuts
{
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建系统字典项时的输入或输出参数类型。
    /// </summary>
    public interface IDicItemCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        Guid DicId { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
