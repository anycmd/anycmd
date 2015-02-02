
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoRuleCommand : UpdateEntityCommand<IInfoRuleUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoRuleCommand(IAcSession userSession, IInfoRuleUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
