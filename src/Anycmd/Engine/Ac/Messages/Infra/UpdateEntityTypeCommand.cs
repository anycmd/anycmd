
namespace Anycmd.Engine.Ac.Messages.Infra
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
