
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateUiViewCommand : UpdateEntityCommand<IUiViewUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewCommand(IUserSession userSession, IUiViewUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
