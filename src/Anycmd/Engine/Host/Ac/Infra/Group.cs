
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Groups;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示组数据访问实体。
    /// </summary>
    public class Group : GroupBase, IAggregateRoot
    {
        public Group()
        {
            base.IsEnabled = 1;
        }

        public static Group Create(IGroupCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Group
            {
                Id = input.Id.Value,
                CategoryCode = input.CategoryCode,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
                Name = input.Name,
                ShortName = input.ShortName,
                SortCode = input.SortCode,
                TypeCode = input.TypeCode
            };
        }

        public void Update(IGroupUpdateIo input)
        {
            this.CategoryCode = input.CategoryCode;
            this.Description = input.Description;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.ShortName = input.ShortName;
            this.SortCode = input.SortCode;
        }
    }
}
