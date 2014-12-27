
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class UpdateFunctionCommand : UpdateEntityCommand<IFunctionUpdateIo>, ISysCommand
    {
        public UpdateFunctionCommand(IFunctionUpdateIo input)
            : base(input)
        {

        }
    }
}
