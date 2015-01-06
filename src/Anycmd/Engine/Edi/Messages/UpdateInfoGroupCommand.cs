
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoGroupCommand : UpdateEntityCommand<IInfoGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoGroupCommand(IInfoGroupUpdateIo input)
            : base(input)
        {

        }
    }
}
