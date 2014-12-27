
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示静态职责分离角色集数据访问实体。
    /// </summary>
    public class SsdSet : SsdSetBase, IAggregateRoot
    {
        public static SsdSet Create(ISsdSetCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new SsdSet
            {
                Id = input.Id.Value,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
                Name = input.Name,
                SsdCard = input.SsdCard
            };
        }

        public void Update(ISsdSetUpdateIo input)
        {
            this.Description = input.Description;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.SsdCard = input.SsdCard;
        }
    }
}
