
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddFunctionCommand : AddEntityCommand<IFunctionCreateIo>, ISysCommand
    {
        public AddFunctionCommand(IFunctionCreateIo input)
            : base(input)
        {

        }
    }
}
