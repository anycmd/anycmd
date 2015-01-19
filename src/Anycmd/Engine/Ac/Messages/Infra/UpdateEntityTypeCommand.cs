
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateEntityTypeCommand : UpdateEntityCommand<IEntityTypeUpdateIo>, IAnycmdCommand
    {
        public UpdateEntityTypeCommand(IUserSession userSession, IEntityTypeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
