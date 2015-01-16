
namespace Anycmd.Ac.ViewModels.DsdViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;

    /// <summary>
    /// 创建动态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public class DsdSetCreateIo : EntityCreateInput, IDsdSetCreateIo
    {
        public DsdSetCreateIo()
        {
            OntologyCode = "DsdSet";
            Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public string Name { get; set; }

        public int DsdCard { get; set; }

        public int IsEnabled { get; set; }

        public string Description { get; set; }

        public AddDsdSetCommand ToCommand()
        {
            return new AddDsdSetCommand(this);
        }
    }
}
