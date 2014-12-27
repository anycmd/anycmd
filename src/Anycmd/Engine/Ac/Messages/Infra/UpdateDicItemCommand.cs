
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateDicItemCommand : UpdateEntityCommand<IDicItemUpdateIo>, ISysCommand
    {
        public UpdateDicItemCommand(IDicItemUpdateIo input)
            : base(input)
        {

        }
    }
}
