
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOntologyCommand : AddEntityCommand<IOntologyCreateIo>, IAnycmdCommand
    {
        public AddOntologyCommand(IOntologyCreateIo input)
            : base(input)
        {

        }
    }
}
