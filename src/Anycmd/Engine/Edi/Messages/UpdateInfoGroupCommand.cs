
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoGroupCommand : UpdateEntityCommand<IInfoGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoGroupCommand(IUserSession userSession, IInfoGroupUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
