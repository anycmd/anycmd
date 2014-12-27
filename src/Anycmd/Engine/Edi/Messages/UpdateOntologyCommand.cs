
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateOntologyCommand: UpdateEntityCommand<IOntologyUpdateIo>, ISysCommand
    {
        public UpdateOntologyCommand(IOntologyUpdateIo input)
            : base(input)
        {

        }
    }
}
