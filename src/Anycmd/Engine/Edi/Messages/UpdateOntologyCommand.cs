
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateOntologyCommand : UpdateEntityCommand<IOntologyUpdateIo>, IAnycmdCommand
    {
        public UpdateOntologyCommand(IAcSession acSession, IOntologyUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
