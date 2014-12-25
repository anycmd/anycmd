
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示系统字典项数据访问实体。
    /// </summary>
    public class DicItem : DicItemBase, IAggregateRoot
    {
        public DicItem()
        {
        }

        public static DicItem Create(IDicItemCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new DicItem
            {
                Id = input.Id.Value,
                Code = input.Code,
                Name = input.Name,
                DicId = input.DicId,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
                SortCode = input.SortCode
            };
        }

        public void Update(IDicItemUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
