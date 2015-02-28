
namespace Anycmd.Ac.ViewModels.SsdViewModels
{
    using Engine.Ac.Ssd;
    using Engine.Messages;
    using System;

    /// <summary>
    /// 更新静态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public class SsdSetUpdateIo : ISsdSetUpdateIo
    {
        public SsdSetUpdateIo()
        {
            HecpOntology = "SsdSet";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public string Name { get; set; }

        public int SsdCard { get; set; }

        public int IsEnabled { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateSsdSetCommand(acSession, this);
        }
    }
}
