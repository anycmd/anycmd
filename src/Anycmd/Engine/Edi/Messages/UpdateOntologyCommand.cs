
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateOntologyCommand: UpdateEntityCommand<IOntologyUpdateIo>, IAnycmdCommand
    {
        public UpdateOntologyCommand(IAcSession userSession, IOntologyUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
