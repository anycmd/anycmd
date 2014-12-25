
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;
    using System;

    /// <summary>
    /// 创建静态职责分离角色时的输入或输出参数类型。
    /// </summary>
    public class SsdRoleCreateIo : EntityCreateInput, IInputModel, ISsdRoleCreateIo
    {
        public Guid SsdSetId { get; set; }

        public Guid RoleId { get; set; }
    }
}
