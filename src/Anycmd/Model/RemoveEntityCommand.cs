
namespace Anycmd.Model
{
    using Commands;
    using System;

    /// <summary>
    /// 表示表达移除给定的实体的命令的.NET类型。要定位一个实体需要两个信息：1 实体的类型；2 实体的标识。其中实体类型由.NET类型命名规则得知。
    /// <remarks>.NET类型命名规则：Remove'EntityType'Command。如RemoveUserCommand、RemoveRoleCommand、RemoveOrganizationCommand。</remarks>
    /// </summary>
    public abstract class RemoveEntityCommand : Command
    {
        protected RemoveEntityCommand(Guid entityId)
        {
            this.EntityId = entityId;
        }

        /// <summary>
        /// 实体标识。
        /// </summary>
        public Guid EntityId { get; private set; }
    }
}
