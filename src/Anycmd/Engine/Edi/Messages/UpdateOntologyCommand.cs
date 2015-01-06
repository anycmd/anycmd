
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateOntologyCommand: UpdateEntityCommand<IOntologyUpdateIo>, IAnycmdCommand
    {
        public UpdateOntologyCommand(IOntologyUpdateIo input)
            : base(input)
        {

        }
    }
}
