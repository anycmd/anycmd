
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddOntologyCommand : AddEntityCommand<IOntologyCreateIo>, ISysCommand
    {
        public AddOntologyCommand(IOntologyCreateIo input)
            : base(input)
        {

        }
    }
}
