
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示动态职责分离角色集数据访问实体。
    /// </summary>
    public class DsdSet : DsdSetBase, IAggregateRoot
    {
        public static DsdSet Create(IDsdSetCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new DsdSet
            {
                Id = input.Id.Value,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
                Name = input.Name,
                DsdCard = input.DsdCard
            };
        }

        public void Update(IDsdSetUpdateIo input)
        {
            this.Description = input.Description;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.DsdCard = input.DsdCard;
        }
    }
}
