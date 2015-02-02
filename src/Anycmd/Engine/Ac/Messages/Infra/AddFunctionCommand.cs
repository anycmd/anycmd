
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddFunctionCommand : AddEntityCommand<IFunctionCreateIo>, IAnycmdCommand
    {
        public AddFunctionCommand(IAcSession userSession, IFunctionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
