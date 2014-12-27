
namespace Anycmd.Engine.Ac.InOuts
{
    using Model;
    using System;

    /// <summary>
    /// 表示改接口的实现类是创建系统资源类型时的输入或输出参数类型。
    /// </summary>
    public interface IResourceTypeCreateIo : IEntityCreateInput
    {
        Guid AppSystemId { get; }
        string Code { get; set; }
        string Description { get; }
        string Icon { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
