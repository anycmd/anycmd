
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示话题数据访问实体。
    /// </summary>
    public class Topic : TopicBase, IAggregateRoot
    {
        public Topic() { }

        public static Topic Create(ITopicCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Topic
            {
                Code = input.Code,
                Id = input.Id.Value,
                Description = input.Description,
                IsAllowed = input.IsAllowed,
                Name = input.Name,
                OntologyId = input.OntologyId
            };
        }

        public void Update(ITopicUpdateIo input)
        {
            this.Code = input.Code;
            this.Name = input.Name;
            this.Description = input.Description;
        }
    }
}
