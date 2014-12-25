
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;
    using System;

    /// <summary>
    /// 创建动态职责分离角色时的输入或输出参数类型。
    /// </summary>
    public class DsdRoleCreateIo : EntityCreateInput, IInputModel, IDsdRoleCreateIo
    {
        public Guid DsdSetId { get; set; }

        public Guid RoleId { get; set; }
    }
}
