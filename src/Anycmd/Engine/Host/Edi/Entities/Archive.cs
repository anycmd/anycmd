
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 表示归档记录数据访问实体。
    /// </summary>
    public class Archive : ArchiveBase, IAggregateRoot
    {
        public Archive() { }

        public static Archive Create(IArchiveCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Archive
            {
                Id = input.Id.Value,
                ArchiveOn = DateTime.Now,
                DataSource = string.Empty,
                Description = input.Description,
                FilePath = string.Empty,
                Password = string.Empty,
                Title = input.Title,
                RdbmsType = input.RdbmsType,
                OntologyId = input.OntologyId,
                UserId = string.Empty
            };
        }

        public void Update(IArchiveUpdateIo input)
        {
            this.Description = input.Description;
            this.Title = input.Title;
        }
    }
}
