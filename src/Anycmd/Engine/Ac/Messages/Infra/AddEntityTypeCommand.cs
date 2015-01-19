
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddEntityTypeCommand : AddEntityCommand<IEntityTypeCreateIo>, IAnycmdCommand
    {
        public AddEntityTypeCommand(IUserSession userSession, IEntityTypeCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
