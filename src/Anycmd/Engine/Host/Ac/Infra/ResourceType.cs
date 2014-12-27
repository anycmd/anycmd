
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示资源类型数据访问实体。
    /// </summary>
    public class ResourceType : ResourceTypeBase, IAggregateRoot
    {
        public static ResourceType Create(IResourceTypeCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new ResourceType
            {
                Id = input.Id.Value,
                Code = input.Code,
                AppSystemId = input.AppSystemId,
                Icon = input.Icon,
                Description = input.Description,
                Name = input.Name,
                SortCode = input.SortCode
            };
        }

        public void Update(IResourceTypeUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.Icon = input.Icon;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
