
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateProcessCommand : UpdateEntityCommand<IProcessUpdateIo>, IAnycmdCommand
    {
        public UpdateProcessCommand(IUserSession userSession, IProcessUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
