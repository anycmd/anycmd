
namespace Anycmd.Ac.ViewModels.DsdViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;
    using System;

    /// <summary>
    /// 更新动态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public class DsdSetUpdateIo : IDsdSetUpdateIo
    {
        public DsdSetUpdateIo()
        {
            OntologyCode = "DsdSet";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int DsdCard { get; set; }

        public int IsEnabled { get; set; }

        public string Description { get; set; }

        public UpdateDsdSetCommand ToCommand()
        {
            return new UpdateDsdSetCommand(this);
        }
    }
}
