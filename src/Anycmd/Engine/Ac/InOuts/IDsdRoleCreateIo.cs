
namespace Anycmd.Engine.Ac.InOuts
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建动态职责分离角色时的输入或输出参数类型。
    /// </summary>
    public interface IDsdRoleCreateIo : IEntityCreateInput
    {
        Guid DsdSetId { get; }
        Guid RoleId { get; }
    }
}
