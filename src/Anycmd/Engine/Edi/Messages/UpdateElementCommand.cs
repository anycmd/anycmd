
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateElementCommand : UpdateEntityCommand<IElementUpdateIo>, IAnycmdCommand
    {
        public UpdateElementCommand(IUserSession userSession, IElementUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
