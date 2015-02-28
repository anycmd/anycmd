
namespace Anycmd.Engine.Ac.Functions
{
    using Messages;

    public class UpdateFunctionCommand : UpdateEntityCommand<IFunctionUpdateIo>, IAnycmdCommand
    {
        public UpdateFunctionCommand(IAcSession acSession, IFunctionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
