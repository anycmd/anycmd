
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Edi;
    using System;

    public class ActionTr
    {
        public ActionTr() { }

        public static ActionTr Create(ActionState action)
        {
            return new ActionTr
            {
                Id = action.Id,
                Description = action.Description,
                IsAllowed = action.IsAllowed,
                IsAudit = action.IsAudit,
                IsPersist = action.IsPersist,
                Name = action.Name,
                OntologyId = action.OntologyId,
                Verb = action.Verb,
                CreateOn = action.CreateOn,
                SortCode = action.SortCode
            };
        }

        public Guid Id { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Verb { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否允许
        /// </summary>
        public string IsAllowed { get; set; }
        /// <summary>
        /// 本体主键
        /// </summary>
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public string IsAudit { get; set; }
        /// <summary>
        /// 是否持久化到数据库
        /// </summary>
        public bool IsPersist { get; set; }

        public int SortCode { get; set; }

        public DateTime? CreateOn { get; set; }
    }
}
