
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示信息字典项数据访问实体。
    /// <remarks>
    /// 有些本体元素在实体上的取值不是任意的。当本体是“人”时，人有“民族”这个本体元素，本体元素“民族”的“数据类型”是字典型的。
    /// “教师”本体是“人”本体的一个子类，张老师是一个“实体人”，张老师的“民族属性”取值就不是任意的而是由教育部的“民族”字典限定的。
    /// </remarks>
    /// </summary>
    public class InfoDicItem : InfoDicItemBase, IAggregateRoot
    {
        public InfoDicItem() { }

        public static InfoDicItem Create(IInfoDicItemCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new InfoDicItem
            {
                Code = input.Code,
                Description = input.Description,
                Id = input.Id.Value,
                InfoDicId = input.InfoDicId,
                IsEnabled = input.IsEnabled,
                Level = input.Level,
                Name = input.Name,
                SortCode = input.SortCode
            };
        }

        public void Update(IInfoDicItemUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.InfoDicId = input.InfoDicId;
            this.IsEnabled = input.IsEnabled;
            this.Level = input.Level;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
