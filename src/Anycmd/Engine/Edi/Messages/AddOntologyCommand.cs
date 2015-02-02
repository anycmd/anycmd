
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOntologyCommand : AddEntityCommand<IOntologyCreateIo>, IAnycmdCommand
    {
        public AddOntologyCommand(IAcSession userSession, IOntologyCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
