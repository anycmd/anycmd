
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddUiViewButtonCommand : AddEntityCommand<IUiViewButtonCreateIo>, IAnycmdCommand
    {
        public AddUiViewButtonCommand(IUserSession userSession, IUiViewButtonCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
