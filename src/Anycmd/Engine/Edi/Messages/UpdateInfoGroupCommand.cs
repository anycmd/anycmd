
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoGroupCommand : UpdateEntityCommand<IInfoGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoGroupCommand(IAcSession acSession, IInfoGroupUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
