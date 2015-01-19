
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
            HecpOntology = "DsdSet";
            HecpVerb = "Create";
        }

        public string Name { get; set; }

        public int DsdCard { get; set; }

        public int IsEnabled { get; set; }

        public string Description { get; set; }

        public AddDsdSetCommand ToCommand(IUserSession userSession)
        {
            return new AddDsdSetCommand(userSession, this);
        }
    }
}
