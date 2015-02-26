
namespace Anycmd.Engine.Ac.Functions
{


    public class AddFunctionCommand : AddEntityCommand<IFunctionCreateIo>, IAnycmdCommand
    {
        public AddFunctionCommand(IAcSession acSession, IFunctionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
