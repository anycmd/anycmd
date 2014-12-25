
namespace Anycmd.Engine.Host.Edi.Messages
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
