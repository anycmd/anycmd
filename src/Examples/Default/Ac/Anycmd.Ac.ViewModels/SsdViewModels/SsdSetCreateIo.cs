
namespace Anycmd.Ac.ViewModels.SsdViewModels
{
    using Engine.Ac.Ssd;
    using Engine.InOuts;
    using Engine.Messages;

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

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddSsdSetCommand(acSession, this);
        }
    }
}
