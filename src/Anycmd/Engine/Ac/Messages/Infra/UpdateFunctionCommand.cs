
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateFunctionCommand : UpdateEntityCommand<IFunctionUpdateIo>, IAnycmdCommand
    {
        public UpdateFunctionCommand(IUserSession userSession, IFunctionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
