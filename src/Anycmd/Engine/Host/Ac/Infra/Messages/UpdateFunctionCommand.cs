
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
