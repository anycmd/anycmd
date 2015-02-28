
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class AddInfoGroupCommand : AddEntityCommand<IInfoGroupCreateIo>, IAnycmdCommand
    {
        public AddInfoGroupCommand(IAcSession acSession, IInfoGroupCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
