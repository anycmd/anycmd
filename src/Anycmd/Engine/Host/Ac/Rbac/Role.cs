
namespace Anycmd.Engine.Host.Ac.Rbac
{
    using Engine.Ac.Roles;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示角色数据访问实体。
    /// </summary>
    public class Role : RoleBase, IAggregateRoot
    {
        public Role()
        {
            base.IsEnabled = 1;
        }

        public static Role Create(IRoleCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Role
            {
                CategoryCode = input.CategoryCode,
                Description = input.Description,
                Icon = input.Icon,
                Id = input.Id.Value,
                IsEnabled = input.IsEnabled,
                Name = input.Name,
                SortCode = input.SortCode
            };
        }

        public void Update(IRoleUpdateIo input)
        {
            this.CategoryCode = input.CategoryCode;
            this.Description = input.Description;
            this.Icon = input.Icon;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
