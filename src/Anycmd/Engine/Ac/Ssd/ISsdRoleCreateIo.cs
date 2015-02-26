
namespace Anycmd.Engine.Ac.Ssd
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建静态职责分离角色时的输入或输出参数类型。
    /// </summary>
    public interface ISsdRoleCreateIo : IEntityCreateInput
    {
        Guid SsdSetId { get; }
        Guid RoleId { get; }
    }
}
