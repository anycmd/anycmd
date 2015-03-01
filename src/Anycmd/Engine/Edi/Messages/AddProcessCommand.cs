
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddProcessCommand : AddEntityCommand<IProcessCreateIo>, IAnycmdCommand
    {
        public AddProcessCommand(IAcSession acSession, IProcessCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
