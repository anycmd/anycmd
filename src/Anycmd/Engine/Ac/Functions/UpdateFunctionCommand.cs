
namespace Anycmd.Engine.Ac.Functions
{


    public class UpdateFunctionCommand : UpdateEntityCommand<IFunctionUpdateIo>, IAnycmdCommand
    {
        public UpdateFunctionCommand(IAcSession acSession, IFunctionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
