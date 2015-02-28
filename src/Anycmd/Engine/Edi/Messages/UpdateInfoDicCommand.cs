
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class UpdateInfoDicCommand : UpdateEntityCommand<IInfoDicUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicCommand(IAcSession acSession, IInfoDicUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
