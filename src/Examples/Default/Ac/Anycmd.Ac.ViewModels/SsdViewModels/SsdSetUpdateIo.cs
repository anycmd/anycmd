
namespace Anycmd.Ac.ViewModels.SsdViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;
    using System;

    /// <summary>
    /// 更新静态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public class SsdSetUpdateIo : ISsdSetUpdateIo
    {
        public SsdSetUpdateIo()
        {
            OntologyCode = "SsdSet";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public string Name { get; set; }

        public int SsdCard { get; set; }

        public int IsEnabled { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public UpdateSsdSetCommand ToCommand()
        {
            return new UpdateSsdSetCommand(this);
        }
    }
}
