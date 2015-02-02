
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateFunctionCommand : UpdateEntityCommand<IFunctionUpdateIo>, IAnycmdCommand
    {
        public UpdateFunctionCommand(IAcSession userSession, IFunctionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
