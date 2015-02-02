
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoGroupCommand : UpdateEntityCommand<IInfoGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoGroupCommand(IAcSession userSession, IInfoGroupUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
