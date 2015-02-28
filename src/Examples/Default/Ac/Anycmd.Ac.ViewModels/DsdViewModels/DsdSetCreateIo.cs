
namespace Anycmd.Ac.ViewModels.DsdViewModels
{
    using Engine.Ac.Dsd;
    using Engine.InOuts;
    using Engine.Messages;

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

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddDsdSetCommand(acSession, this);
        }
    }
}
