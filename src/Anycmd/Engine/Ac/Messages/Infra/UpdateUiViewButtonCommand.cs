
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateUiViewButtonCommand : UpdateEntityCommand<IUiViewButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewButtonCommand(IUserSession userSession, IUiViewButtonUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
