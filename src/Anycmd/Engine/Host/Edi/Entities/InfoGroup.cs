
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示信息组数据访问实体。本体元素的分组
    /// </summary>
    public class InfoGroup : InfoGroupBase, IAggregateRoot
    {
        public InfoGroup() { }

        public static InfoGroup Create(IInfoGroupCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new InfoGroup
            {
                Code = input.Code,
                Id = input.Id.Value,
                Description = input.Description,
                Name = input.Name,
                SortCode = input.SortCode,
                OntologyId = input.OntologyId
            };
        }

        public void Update(IInfoGroupUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
