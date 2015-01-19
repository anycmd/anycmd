
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateDicCommand : UpdateEntityCommand<IDicUpdateIo>, IAnycmdCommand
    {
        public UpdateDicCommand(IUserSession userSession, IDicUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
