
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示动作数据访问实体。
    /// </summary>
    public class Action : ActionBase, IAggregateRoot
    {
        public Action() { }

        public static Action Create(IActionCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Action
            {
                IsAllowed = input.IsAllowed,
                IsAudit = input.IsAudit,
                Id = input.Id.Value,
                Description = input.Description,
                IsPersist = input.IsPersist,
                Name = input.Name,
                OntologyId = input.OntologyId,
                SortCode = input.SortCode,
                Verb = input.Verb
            };
        }

        public void Update(IActionUpdateIo input)
        {
            this.Description = input.Description;
            this.IsAllowed = input.IsAllowed;
            this.IsPersist = input.IsPersist;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
            this.Verb = input.Verb;
        }
    }
}
