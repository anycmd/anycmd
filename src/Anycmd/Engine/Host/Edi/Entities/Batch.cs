
namespace Anycmd.Engine.Host.Edi.Entities {
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 表示批数据访问实体。“批”最初是用来批量生成待分发向一线通的Create型命令的。
    /// <remarks>
    /// 从两个角度理解批：1，一个批往往涉及多个命令；2，一个批往往影响多个实体。
    /// </remarks>
    /// </summary>
    public class Batch : EntityBase, IAggregateRoot, IBatch
    {
        public Batch() { }

        public static Batch Create(IBatchCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Batch
            {
                Id = input.Id.Value,
                IncludeDescendants = input.IncludeDescendants,
                NodeId = input.NodeId,
                OntologyId = input.OntologyId,
                OrganizationCode = input.OrganizationCode,
                Title = input.Title,
                Total = 0,
                Type = input.Type,
                Description = input.Description
            };
        }

        public void Update(IBatchUpdateIo input)
        {
            this.Description = input.Description;
            this.Title = input.Title;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IncludeDescendants { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }
    }
}
