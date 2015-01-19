
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdatePropertyCommand : UpdateEntityCommand<IPropertyUpdateIo>, IAnycmdCommand
    {
        public UpdatePropertyCommand(IUserSession userSession, IPropertyUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
