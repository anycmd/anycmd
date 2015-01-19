
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, IAnycmdCommand
    {
        public AddButtonCommand(IUserSession userSession, IButtonCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
