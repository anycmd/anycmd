
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class UpdateEntityTypeCommand : UpdateEntityCommand<IEntityTypeUpdateIo>, ISysCommand
    {
        public UpdateEntityTypeCommand(IEntityTypeUpdateIo input)
            : base(input)
        {

        }
    }
}
