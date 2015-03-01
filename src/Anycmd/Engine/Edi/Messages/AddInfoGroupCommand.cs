
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddInfoGroupCommand : AddEntityCommand<IInfoGroupCreateIo>, IAnycmdCommand
    {
        public AddInfoGroupCommand(IAcSession acSession, IInfoGroupCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
