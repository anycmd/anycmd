
namespace Anycmd.Ac.ViewModels.SsdViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;

    /// <summary>
    /// 创建静态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public class SsdSetCreateIo : EntityCreateInput, ISsdSetCreateIo
    {
        public SsdSetCreateIo()
        {
            HecpOntology = "SsdSet";
            HecpVerb = "Create";
        }

        public string Name { get; set; }

        public int SsdCard { get; set; }

        public int IsEnabled { get; set; }

        public string Description { get; set; }

        public override IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new AddSsdSetCommand(userSession, this);
        }
    }
}
