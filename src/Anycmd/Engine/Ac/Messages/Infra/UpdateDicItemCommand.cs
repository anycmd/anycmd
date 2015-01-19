
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateDicItemCommand : UpdateEntityCommand<IDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateDicItemCommand(IUserSession userSession, IDicItemUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
