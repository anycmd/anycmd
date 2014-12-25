
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class UpdateButtonCommand : UpdateEntityCommand<IButtonUpdateIo>, ISysCommand
    {
        public UpdateButtonCommand(IButtonUpdateIo input)
            : base(input)
        {

        }
    }
}
