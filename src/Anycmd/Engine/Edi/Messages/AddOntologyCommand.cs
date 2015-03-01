
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddOntologyCommand : AddEntityCommand<IOntologyCreateIo>, IAnycmdCommand
    {
        public AddOntologyCommand(IAcSession acSession, IOntologyCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
