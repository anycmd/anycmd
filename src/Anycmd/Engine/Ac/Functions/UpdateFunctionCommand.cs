
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateFunctionCommand : UpdateEntityCommand<IFunctionUpdateIo>, IAnycmdCommand
    {
        public UpdateFunctionCommand(IAcSession acSession, IFunctionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
