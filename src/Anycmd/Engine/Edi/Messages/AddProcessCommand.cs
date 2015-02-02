
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddProcessCommand : AddEntityCommand<IProcessCreateIo>, IAnycmdCommand
    {
        public AddProcessCommand(IAcSession acSession, IProcessCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
