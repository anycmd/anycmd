
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoRuleCommand : UpdateEntityCommand<IInfoRuleUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoRuleCommand(IAcSession acSession, IInfoRuleUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
